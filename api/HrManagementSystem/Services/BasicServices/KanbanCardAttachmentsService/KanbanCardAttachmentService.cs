using HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;

namespace HrManagementSystem.Services.BasicServices.KanbanCardAttachmentsService;

public class KanbanCardAttachmentService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanCardAttachmentErrors kanbanCardAttachmentErrors) : IKanbanCardAttachmentService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanCardAttachmentErrors _kanbanCardAttachmentErrors = kanbanCardAttachmentErrors;

    public async Task<IEnumerable<KanbanCardAttachmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardAttachments
            .AsNoTracking()
            .Where(a => !a.IsDeleted)
            .Include(a => a.KanbanCard)
            .Include(a => a.UploadedFile)
            .Select(a => new KanbanCardAttachmentResponse(
                a.Id,
                a.KanbanCardId,
                a.KanbanCard.Title,
                a.UploadedFileId,
                a.UploadedFile.FileName,
                a.UploadedFile.StoredFileName,
                0L,
                a.UploadedFile.ContentType,
                a.CreatedOn,
                a.CreatedOn,
                a.UpdatedOn,
                a.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardAttachmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var attachment = await _context.KanbanCardAttachments
            .AsNoTracking()
            .Where(a => a.Id == id && !a.IsDeleted)
            .Include(a => a.KanbanCard)
            .Include(a => a.UploadedFile)
            .Select(a => new KanbanCardAttachmentResponse(
                a.Id,
                a.KanbanCardId,
                a.KanbanCard.Title,
                a.UploadedFileId,
                a.UploadedFile.FileName,
                a.UploadedFile.StoredFileName,
                0L,
                a.UploadedFile.ContentType,
                a.CreatedOn,
                a.CreatedOn,
                a.UpdatedOn,
                a.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (attachment is null)
            return Result.Failure<KanbanCardAttachmentResponse>(_kanbanCardAttachmentErrors.KanbanCardAttachmentNotFound);

        return Result.Success(attachment);
    }

    public async Task<IEnumerable<KanbanCardAttachmentResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardAttachments
            .AsNoTracking()
            .Where(a => a.KanbanCardId == cardId && !a.IsDeleted)
            .Include(a => a.KanbanCard)
            .Include(a => a.UploadedFile)
            .Select(a => new KanbanCardAttachmentResponse(
                a.Id,
                a.KanbanCardId,
                a.KanbanCard.Title,
                a.UploadedFileId,
                a.UploadedFile.FileName,
                a.UploadedFile.StoredFileName,
                0L,
                a.UploadedFile.ContentType,
                a.CreatedOn,
                a.CreatedOn,
                a.UpdatedOn,
                a.IsDeleted
            ))
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardAttachmentResponse>> AddAsync(KanbanCardAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var newAttachment = _mapper.Map<KanbanCardAttachment>(request);
        await _context.KanbanCardAttachments.AddAsync(newAttachment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardAttachments
            .AsNoTracking()
            .Where(a => a.Id == newAttachment.Id)
            .Include(a => a.KanbanCard)
            .Include(a => a.UploadedFile)
            .Select(a => new KanbanCardAttachmentResponse(
                a.Id,
                a.KanbanCardId,
                a.KanbanCard.Title,
                a.UploadedFileId,
                a.UploadedFile.FileName,
                a.UploadedFile.StoredFileName,
                0L,
                a.UploadedFile.ContentType,
                a.CreatedOn,
                a.CreatedOn,
                a.UpdatedOn,
                a.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanCardAttachmentResponse>> UpdateAsync(KanbanCardAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanCardAttachments
            .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanCardAttachmentResponse>(_kanbanCardAttachmentErrors.KanbanCardAttachmentNotFound);

        var updated = _mapper.Map<KanbanCardAttachment>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);
        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardAttachments
            .AsNoTracking()
            .Where(a => a.Id == current.Id)
            .Include(a => a.KanbanCard)
            .Include(a => a.UploadedFile)
            .Select(a => new KanbanCardAttachmentResponse(
                a.Id,
                a.KanbanCardId,
                a.KanbanCard.Title,
                a.UploadedFileId,
                a.UploadedFile.FileName,
                a.UploadedFile.StoredFileName,
                0L,
                a.UploadedFile.ContentType,
                a.CreatedOn,
                a.CreatedOn,
                a.UpdatedOn,
                a.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var attachment = await _context.KanbanCardAttachments
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (attachment == null)
            return Result.Failure(_kanbanCardAttachmentErrors.KanbanCardAttachmentNotFound);

        attachment.IsDeleted = !attachment.IsDeleted;
        attachment.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        attachment.DeletedOn = DateTime.UtcNow;
        attachment.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
