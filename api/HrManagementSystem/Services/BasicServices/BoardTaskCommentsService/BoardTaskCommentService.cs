using HrManagementSystem.Contracts.BasicContracts.BoardTaskComments;

namespace HrManagementSystem.Services.BasicServices.BoardTaskCommentsService;

public class BoardTaskCommentService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    BoardTaskCommentErrors boardTaskCommentErrors) : IBoardTaskCommentService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly BoardTaskCommentErrors _boardTaskCommentErrors = boardTaskCommentErrors;

    public async Task<IEnumerable<BoardTaskCommentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BoardTaskComments
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Include(c => c.BoardTask)
            .Include(c => c.User)
            .Select(c => new BoardTaskCommentResponse(
                c.Id,
                c.BoardTaskId,
                c.BoardTask.Title,
                c.CommentText,
                c.UserId,
                c.User.UserName!,
                c.User.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<BoardTaskCommentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.BoardTaskComments
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Include(c => c.BoardTask)
            .Include(c => c.User)
            .Select(c => new BoardTaskCommentResponse(
                c.Id,
                c.BoardTaskId,
                c.BoardTask.Title,
                c.CommentText,
                c.UserId,
                c.User.UserName!,
                c.User.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (comment is null)
            return Result.Failure<BoardTaskCommentResponse>(_boardTaskCommentErrors.BoardTaskCommentNotFound);

        return Result.Success(comment);
    }

    public async Task<IEnumerable<BoardTaskCommentResponse>> GetByTaskIdAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardTaskComments
            .AsNoTracking()
            .Where(c => c.BoardTaskId == taskId && !c.IsDeleted)
            .Include(c => c.BoardTask)
            .Include(c => c.User)
            .Select(c => new BoardTaskCommentResponse(
                c.Id,
                c.BoardTaskId,
                c.BoardTask.Title,
                c.CommentText,
                c.UserId,
                c.User.UserName!,
                c.User.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<BoardTaskCommentResponse>> AddAsync(BoardTaskCommentRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        var newComment = _mapper.Map<BoardTaskComment>(request);
        newComment.UserId = currentUserId;

        await _context.BoardTaskComments.AddAsync(newComment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTaskComments
            .AsNoTracking()
            .Where(c => c.Id == newComment.Id)
            .Include(c => c.BoardTask)
            .Include(c => c.User)
            .Select(c => new BoardTaskCommentResponse(
                c.Id,
                c.BoardTaskId,
                c.BoardTask.Title,
                c.CommentText,
                c.UserId,
                c.User.UserName!,
                c.User.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<BoardTaskCommentResponse>> UpdateAsync(BoardTaskCommentRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();

        var current = await _context.BoardTaskComments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (current == null)
            return Result.Failure<BoardTaskCommentResponse>(_boardTaskCommentErrors.BoardTaskCommentNotFound);

        if (current.UserId != currentUserId)
            return Result.Failure<BoardTaskCommentResponse>(_boardTaskCommentErrors.UnauthorizedCommentAccess);

        var updated = _mapper.Map<BoardTaskComment>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);
        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTaskComments
            .AsNoTracking()
            .Where(c => c.Id == current.Id)
            .Include(c => c.BoardTask)
            .Include(c => c.User)
            .Select(c => new BoardTaskCommentResponse(
                c.Id,
                c.BoardTaskId,
                c.BoardTask.Title,
                c.CommentText,
                c.UserId,
                c.User.UserName!,
                c.User.Email!,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();

        var comment = await _context.BoardTaskComments
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (comment == null)
            return Result.Failure(_boardTaskCommentErrors.BoardTaskCommentNotFound);

        if (comment.UserId != currentUserId)
            return Result.Failure(_boardTaskCommentErrors.UnauthorizedCommentAccess);

        comment.IsDeleted = !comment.IsDeleted;
        comment.DeletedById = currentUserId;
        comment.DeletedOn = DateTime.UtcNow;
        comment.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
