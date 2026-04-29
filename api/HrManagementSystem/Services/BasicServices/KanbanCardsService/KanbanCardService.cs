using HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

namespace HrManagementSystem.Services.BasicServices.KanbanCardsService;

public class KanbanCardService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanCardErrors kanbanCardErrors) : IKanbanCardService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanCardErrors _kanbanCardErrors = kanbanCardErrors;

    public async Task<IEnumerable<KanbanCardResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCards
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Order)
            .Select(c => new KanbanCardResponse(
                c.Id,
                c.KanbanColumnId,
                c.Title,
                c.Description,
                c.Order,
                c.DueDate,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Assignees
                    .Where(a => !a.IsDeleted)
                    .Select(a => new KanbanCardAssigneeResponse(
                        a.Id,
                        a.KanbanCardId,
                        a.KanbanCard.Title,
                        a.UserId,
                        a.User.UserName ?? string.Empty,
                        a.User.Email ?? string.Empty,
                        a.CreatedOn,
                        a.UpdatedOn,
                        a.IsDeleted
                    )),
                c.CardLabels
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new SimpleKanbanCardLabelResponse(
                        cl.Id,
                        cl.KanbanCardId,
                        cl.KanbanLabelId,
                        cl.KanbanLabel.Name,
                        cl.KanbanLabel.ColorHex
                    )),
                c.Comments
                    .Where(com => !com.IsDeleted)
                    .OrderByDescending(com => com.CreatedOn)
                    .Select(com => new KanbanCardCommentResponse(
                        com.Id,
                        com.KanbanCardId,
                        com.KanbanCard.Title,
                        com.CommentText,
                        com.CreatedById ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.UserName).FirstOrDefault() ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.Email).FirstOrDefault() ?? string.Empty,
                        com.CreatedOn,
                        com.UpdatedOn,
                        com.IsDeleted
                    )),
                c.Attachments
                    .Where(att => !att.IsDeleted)
                    .OrderByDescending(att => att.CreatedOn)
                    .Select(att => new KanbanCardAttachmentResponse(
                        att.Id,
                        att.KanbanCardId,
                        att.KanbanCard.Title,
                        att.UploadedFileId,
                        att.UploadedFile.FileName,
                        att.UploadedFile.StoredFileName,
                        0L,
                        att.UploadedFile.ContentType,
                        att.CreatedOn,
                        att.CreatedOn,
                        att.UpdatedOn,
                        att.IsDeleted
                    ))
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var card = await _context.KanbanCards
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new KanbanCardResponse(
                c.Id,
                c.KanbanColumnId,
                c.Title,
                c.Description,
                c.Order,
                c.DueDate,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Assignees
                    .Where(a => !a.IsDeleted)
                    .Select(a => new KanbanCardAssigneeResponse(
                        a.Id,
                        a.KanbanCardId,
                        a.KanbanCard.Title,
                        a.UserId,
                        a.User.UserName ?? string.Empty,
                        a.User.Email ?? string.Empty,
                        a.CreatedOn,
                        a.UpdatedOn,
                        a.IsDeleted
                    )),
                c.CardLabels
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new SimpleKanbanCardLabelResponse(
                        cl.Id,
                        cl.KanbanCardId,
                        cl.KanbanLabelId,
                        cl.KanbanLabel.Name,
                        cl.KanbanLabel.ColorHex
                    )),
                c.Comments
                    .Where(com => !com.IsDeleted)
                    .OrderByDescending(com => com.CreatedOn)
                    .Select(com => new KanbanCardCommentResponse(
                        com.Id,
                        com.KanbanCardId,
                        com.KanbanCard.Title,
                        com.CommentText,
                        com.CreatedById ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.UserName).FirstOrDefault() ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.Email).FirstOrDefault() ?? string.Empty,
                        com.CreatedOn,
                        com.UpdatedOn,
                        com.IsDeleted
                    )),
                c.Attachments
                    .Where(att => !att.IsDeleted)
                    .OrderByDescending(att => att.CreatedOn)
                    .Select(att => new KanbanCardAttachmentResponse(
                        att.Id,
                        att.KanbanCardId,
                        att.KanbanCard.Title,
                        att.UploadedFileId,
                        att.UploadedFile.FileName,
                        att.UploadedFile.StoredFileName,
                        0L,
                        att.UploadedFile.ContentType,
                        att.CreatedOn,
                        att.CreatedOn,
                        att.UpdatedOn,
                        att.IsDeleted
                    ))
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (card is null)
            return Result.Failure<KanbanCardResponse>(_kanbanCardErrors.KanbanCardNotFound);

        return Result.Success(card);
    }

    public async Task<Result<KanbanCardResponse>> AddAsync(KanbanCardRequest request, CancellationToken cancellationToken = default)
    {
        var newCard = _mapper.Map<KanbanCard>(request);

        await _context.KanbanCards.AddAsync(newCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCards
            .AsNoTracking()
            .Where(c => c.Id == newCard.Id)
            .Select(c => new KanbanCardResponse(
                c.Id,
                c.KanbanColumnId,
                c.Title,
                c.Description,
                c.Order,
                c.DueDate,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Assignees
                    .Where(a => !a.IsDeleted)
                    .Select(a => new KanbanCardAssigneeResponse(
                        a.Id,
                        a.KanbanCardId,
                        a.KanbanCard.Title,
                        a.UserId,
                        a.User.UserName ?? string.Empty,
                        a.User.Email ?? string.Empty,
                        a.CreatedOn,
                        a.UpdatedOn,
                        a.IsDeleted
                    )),
                c.CardLabels
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new SimpleKanbanCardLabelResponse(
                        cl.Id,
                        cl.KanbanCardId,
                        cl.KanbanLabelId,
                        cl.KanbanLabel.Name,
                        cl.KanbanLabel.ColorHex
                    )),
                c.Comments
                    .Where(com => !com.IsDeleted)
                    .OrderByDescending(com => com.CreatedOn)
                    .Select(com => new KanbanCardCommentResponse(
                        com.Id,
                        com.KanbanCardId,
                        com.KanbanCard.Title,
                        com.CommentText,
                        com.CreatedById ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.UserName).FirstOrDefault() ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.Email).FirstOrDefault() ?? string.Empty,
                        com.CreatedOn,
                        com.UpdatedOn,
                        com.IsDeleted
                    )),
                c.Attachments
                    .Where(att => !att.IsDeleted)
                    .OrderByDescending(att => att.CreatedOn)
                    .Select(att => new KanbanCardAttachmentResponse(
                        att.Id,
                        att.KanbanCardId,
                        att.KanbanCard.Title,
                        att.UploadedFileId,
                        att.UploadedFile.FileName,
                        att.UploadedFile.StoredFileName,
                        0L,
                        att.UploadedFile.ContentType,
                        att.CreatedOn,
                        att.CreatedOn,
                        att.UpdatedOn,
                        att.IsDeleted
                    ))
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanCardResponse>> UpdateAsync(KanbanCardRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanCards
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanCardResponse>(_kanbanCardErrors.KanbanCardNotFound);

        var updated = _mapper.Map<KanbanCard>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCards
            .Where(c => c.Id == updated.Id)
            .Select(c => new KanbanCardResponse(
                c.Id,
                c.KanbanColumnId,
                c.Title,
                c.Description,
                c.Order,
                c.DueDate,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Assignees
                    .Where(a => !a.IsDeleted)
                    .Select(a => new KanbanCardAssigneeResponse(
                        a.Id,
                        a.KanbanCardId,
                        a.KanbanCard.Title,
                        a.UserId,
                        a.User.UserName ?? string.Empty,
                        a.User.Email ?? string.Empty,
                        a.CreatedOn,
                        a.UpdatedOn,
                        a.IsDeleted
                    )),
                c.CardLabels
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new SimpleKanbanCardLabelResponse(
                        cl.Id,
                        cl.KanbanCardId,
                        cl.KanbanLabelId,
                        cl.KanbanLabel.Name,
                        cl.KanbanLabel.ColorHex
                    )),
                c.Comments
                    .Where(com => !com.IsDeleted)
                    .OrderByDescending(com => com.CreatedOn)
                    .Select(com => new KanbanCardCommentResponse(
                        com.Id,
                        com.KanbanCardId,
                        com.KanbanCard.Title,
                        com.CommentText,
                        com.CreatedById ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.UserName).FirstOrDefault() ?? string.Empty,
                        _context.Users.Where(u => u.Id == com.CreatedById).Select(u => u.Email).FirstOrDefault() ?? string.Empty,
                        com.CreatedOn,
                        com.UpdatedOn,
                        com.IsDeleted
                    )),
                c.Attachments
                    .Where(att => !att.IsDeleted)
                    .OrderByDescending(att => att.CreatedOn)
                    .Select(att => new KanbanCardAttachmentResponse(
                        att.Id,
                        att.KanbanCardId,
                        att.KanbanCard.Title,
                        att.UploadedFileId,
                        att.UploadedFile.FileName,
                        att.UploadedFile.StoredFileName,
                        0L,
                        att.UploadedFile.ContentType,
                        att.CreatedOn,
                        att.CreatedOn,
                        att.UpdatedOn,
                        att.IsDeleted
                    ))
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var card = await _context.KanbanCards
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (card == null)
            return Result.Failure(_kanbanCardErrors.KanbanCardNotFound);

        // Prevent deletion if card has active assignees
        var hasActiveAssignees = await _context.KanbanCardAssignees
            .AnyAsync(a => a.KanbanCardId == id && !a.IsDeleted, cancellationToken);
        
        if (hasActiveAssignees)
            return Result.Failure(_kanbanCardErrors.KanbanCardHasAssignees);

        card.IsDeleted = !card.IsDeleted;
        card.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        card.DeletedOn = DateTime.UtcNow;
        card.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
