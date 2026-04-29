using HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;

namespace HrManagementSystem.Services.BasicServices.BoardTaskAttachmentsService;

public class BoardTaskAttachmentService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    BoardTaskAttachmentErrors boardTaskAttachmentErrors) : IBoardTaskAttachmentService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly BoardTaskAttachmentErrors _boardTaskAttachmentErrors = boardTaskAttachmentErrors;

    public async Task<IEnumerable<BoardTaskAttachmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BoardTaskAttachments
            .AsNoTracking()
            .Where(a => !a.IsDeleted)
            .Include(a => a.BoardTask)
            .Include(a => a.UploadedFile)
            .Select(a => new BoardTaskAttachmentResponse(
                a.Id,
                a.BoardTaskId,
                a.BoardTask.Title,
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

    public async Task<Result<BoardTaskAttachmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var attachment = await _context.BoardTaskAttachments
            .AsNoTracking()
            .Where(a => a.Id == id && !a.IsDeleted)
            .Include(a => a.BoardTask)
            .Include(a => a.UploadedFile)
            .Select(a => new BoardTaskAttachmentResponse(
                a.Id,
                a.BoardTaskId,
                a.BoardTask.Title,
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
            return Result.Failure<BoardTaskAttachmentResponse>(_boardTaskAttachmentErrors.BoardTaskAttachmentNotFound);

        return Result.Success(attachment);
    }

    public async Task<IEnumerable<BoardTaskAttachmentResponse>> GetByTaskIdAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardTaskAttachments
            .AsNoTracking()
            .Where(a => a.BoardTaskId == taskId && !a.IsDeleted)
            .Include(a => a.BoardTask)
            .Include(a => a.UploadedFile)
            .Select(a => new BoardTaskAttachmentResponse(
                a.Id,
                a.BoardTaskId,
                a.BoardTask.Title,
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

    public async Task<Result<BoardTaskAttachmentResponse>> AddAsync(BoardTaskAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var newAttachment = _mapper.Map<BoardTaskAttachment>(request);
        await _context.BoardTaskAttachments.AddAsync(newAttachment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTaskAttachments
            .AsNoTracking()
            .Where(a => a.Id == newAttachment.Id)
            .Include(a => a.BoardTask)
            .Include(a => a.UploadedFile)
            .Select(a => new BoardTaskAttachmentResponse(
                a.Id,
                a.BoardTaskId,
                a.BoardTask.Title,
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

    public async Task<Result<BoardTaskAttachmentResponse>> UpdateAsync(BoardTaskAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.BoardTaskAttachments
            .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

        if (current == null)
            return Result.Failure<BoardTaskAttachmentResponse>(_boardTaskAttachmentErrors.BoardTaskAttachmentNotFound);

        var updated = _mapper.Map<BoardTaskAttachment>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);
        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTaskAttachments
            .AsNoTracking()
            .Where(a => a.Id == current.Id)
            .Include(a => a.BoardTask)
            .Include(a => a.UploadedFile)
            .Select(a => new BoardTaskAttachmentResponse(
                a.Id,
                a.BoardTaskId,
                a.BoardTask.Title,
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
        var attachment = await _context.BoardTaskAttachments
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (attachment == null)
            return Result.Failure(_boardTaskAttachmentErrors.BoardTaskAttachmentNotFound);

        attachment.IsDeleted = !attachment.IsDeleted;
        attachment.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        attachment.DeletedOn = DateTime.UtcNow;
        attachment.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
