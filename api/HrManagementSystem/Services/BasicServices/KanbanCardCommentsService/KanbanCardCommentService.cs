using HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;

namespace HrManagementSystem.Services.BasicServices.KanbanCardCommentsService;

public class KanbanCardCommentService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanCardCommentErrors kanbanCardCommentErrors) : IKanbanCardCommentService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanCardCommentErrors _kanbanCardCommentErrors = kanbanCardCommentErrors;

    public async Task<IEnumerable<KanbanCardCommentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardComments
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Include(c => c.KanbanCard)
            .Include(c => c.CreatedBy)
            .Select(c => new KanbanCardCommentResponse(
                c.Id,
                c.KanbanCardId,
                c.KanbanCard.Title,
                c.CommentText,
                c.CreatedById!,
                c.CreatedBy!.UserName!,
                c.CreatedBy.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardCommentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.KanbanCardComments
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Include(c => c.KanbanCard)
            .Include(c => c.CreatedBy)
            .Select(c => new KanbanCardCommentResponse(
                c.Id,
                c.KanbanCardId,
                c.KanbanCard.Title,
                c.CommentText,
                c.CreatedById!,
                c.CreatedBy!.UserName!,
                c.CreatedBy.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (comment is null)
            return Result.Failure<KanbanCardCommentResponse>(_kanbanCardCommentErrors.KanbanCardCommentNotFound);

        return Result.Success(comment);
    }

    public async Task<IEnumerable<KanbanCardCommentResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardComments
            .AsNoTracking()
            .Where(c => c.KanbanCardId == cardId && !c.IsDeleted)
            .Include(c => c.KanbanCard)
            .Include(c => c.CreatedBy)
            .Select(c => new KanbanCardCommentResponse(
                c.Id,
                c.KanbanCardId,
                c.KanbanCard.Title,
                c.CommentText,
                c.CreatedById!,
                c.CreatedBy!.UserName!,
                c.CreatedBy.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardCommentResponse>> AddAsync(KanbanCardCommentRequest request, CancellationToken cancellationToken = default)
    {
        var newComment = _mapper.Map<KanbanCardComment>(request);
        await _context.KanbanCardComments.AddAsync(newComment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardComments
            .AsNoTracking()
            .Where(c => c.Id == newComment.Id)
            .Include(c => c.KanbanCard)
            .Include(c => c.CreatedBy)
            .Select(c => new KanbanCardCommentResponse(
                c.Id,
                c.KanbanCardId,
                c.KanbanCard.Title,
                c.CommentText,
                c.CreatedById!,
                c.CreatedBy!.UserName!,
                c.CreatedBy.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanCardCommentResponse>> UpdateAsync(KanbanCardCommentRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanCardComments
            .Include(c => c.CreatedBy)
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanCardCommentResponse>(_kanbanCardCommentErrors.KanbanCardCommentNotFound);

        // Check if the current user is the comment creator
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (current.CreatedById != currentUserId)
            return Result.Failure<KanbanCardCommentResponse>(_kanbanCardCommentErrors.UnauthorizedCommentAccess);

        var updated = _mapper.Map<KanbanCardComment>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);
        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardComments
            .AsNoTracking()
            .Where(c => c.Id == current.Id)
            .Include(c => c.KanbanCard)
            .Include(c => c.CreatedBy)
            .Select(c => new KanbanCardCommentResponse(
                c.Id,
                c.KanbanCardId,
                c.KanbanCard.Title,
                c.CommentText,
                c.CreatedById!,
                c.CreatedBy!.UserName!,
                c.CreatedBy.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.KanbanCardComments
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (comment == null)
            return Result.Failure(_kanbanCardCommentErrors.KanbanCardCommentNotFound);

        // Check if the current user is the comment creator
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (comment.CreatedById != currentUserId)
            return Result.Failure(_kanbanCardCommentErrors.UnauthorizedCommentAccess);

        comment.IsDeleted = !comment.IsDeleted;
        comment.DeletedById = currentUserId;
        comment.DeletedOn = DateTime.UtcNow;
        comment.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
