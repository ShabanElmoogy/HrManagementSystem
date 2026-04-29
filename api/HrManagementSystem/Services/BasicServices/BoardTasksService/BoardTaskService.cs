using HrManagementSystem.Contracts.BasicContracts.BoardTasks;

namespace HrManagementSystem.Services.BasicServices.BoardTasksService;

public class BoardTaskService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    BoardTaskErrors boardTaskErrors) : IBoardTaskService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly BoardTaskErrors _boardTaskErrors = boardTaskErrors;

    public async Task<IEnumerable<BoardTaskResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BoardTasks
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Select(t => new BoardTaskResponse(
                t.Id,
                t.Title,
                t.Description,
                (int)t.Status,
                (int)t.Priority,
                t.DueDate,
                t.EstimatedHours,
                t.LoggedHours,
                t.AssigneeId,
                t.Position,
                t.KanbanBoardId,
                t.KanbanColumnId,
                t.CreatedOn,
                t.UpdatedOn,
                t.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<BoardTaskResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _context.BoardTasks
            .AsNoTracking()
            .Where(t => t.Id == id && !t.IsDeleted)
            .Select(t => new BoardTaskResponse(
                t.Id,
                t.Title,
                t.Description,
                (int)t.Status,
                (int)t.Priority,
                t.DueDate,
                t.EstimatedHours,
                t.LoggedHours,
                t.AssigneeId,
                t.Position,
                t.KanbanBoardId,
                t.KanbanColumnId,
                t.CreatedOn,
                t.UpdatedOn,
                t.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
            return Result.Failure<BoardTaskResponse>(_boardTaskErrors.BoardTaskNotFound);

        return Result.Success(task);
    }

    public async Task<Result<BoardTaskResponse>> AddAsync(BoardTaskRequest request, CancellationToken cancellationToken = default)
    {
        var newTask = _mapper.Map<BoardTask>(request);

        await _context.BoardTasks.AddAsync(newTask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTasks
            .AsNoTracking()
            .Where(t => t.Id == newTask.Id)
            .Select(t => new BoardTaskResponse(
                t.Id,
                t.Title,
                t.Description,
                (int)t.Status,
                (int)t.Priority,
                t.DueDate,
                t.EstimatedHours,
                t.LoggedHours,
                t.AssigneeId,
                t.Position,
                t.KanbanBoardId,
                t.KanbanColumnId,
                t.CreatedOn,
                t.UpdatedOn,
                t.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<BoardTaskResponse>> UpdateAsync(BoardTaskRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.BoardTasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<BoardTaskResponse>(_boardTaskErrors.BoardTaskNotFound);

        var updated = _mapper.Map<BoardTask>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);
        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.BoardTasks
                                     .Where(t => t.Id == updated.Id)
                                     .Select(t => new BoardTaskResponse(
                                         t.Id,
                                         t.Title,
                                         t.Description,
                                         (int)t.Status,
                                         (int)t.Priority,
                                         t.DueDate,
                                         t.EstimatedHours,
                                         t.LoggedHours,
                                         t.AssigneeId,
                                         t.Position,
                                         t.KanbanBoardId,
                                         t.KanbanColumnId,
                                         t.CreatedOn,
                                         t.UpdatedOn,
                                         t.IsDeleted
                                     ))
                                     .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _context.BoardTasks
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (task == null)
            return Result.Failure(_boardTaskErrors.BoardTaskNotFound);

        // Prevent deletion if task has comments or attachments that are not deleted
        var hasComments = await _context.Set<BoardTaskComment>().AnyAsync(c => c.BoardTaskId == id && !c.IsDeleted, cancellationToken);
        if (hasComments)
            return Result.Failure(_boardTaskErrors.BoardTaskHasComments);

        var hasAttachments = await _context.Set<BoardTaskAttachment>().AnyAsync(a => a.BoardTaskId == id && !a.IsDeleted, cancellationToken);
        if (hasAttachments)
            return Result.Failure(_boardTaskErrors.BoardTaskHasAttachments);

        task.IsDeleted = !task.IsDeleted;
        task.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        task.DeletedOn = DateTime.UtcNow;
        task.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}