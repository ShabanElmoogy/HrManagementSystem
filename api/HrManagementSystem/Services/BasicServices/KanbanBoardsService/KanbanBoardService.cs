using HrManagementSystem.Contracts.BasicContracts.KanbanBoards;

namespace HrManagementSystem.Services.BasicServices.KanbanBoardsService;

public class KanbanBoardService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanBoardErrors kanbanBoardErrors) : IKanbanBoardService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanBoardErrors _kanbanBoardErrors = kanbanBoardErrors;


    public async Task<IEnumerable<KanbanBoardResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanBoards
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Select(c => new KanbanBoardResponse(
                c.Id,
                c.Name,                   
                c.Description,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Columns
                 .Where(col => !col.IsDeleted)
                 .OrderBy(col => col.Order)
                 .Select(col => new KanbanColumnResponse(
                     col.Id,
                     col.Name,
                     col.Order,
                     col.IsArchived,
                     col.Cards
                         .Where(card => !card.IsDeleted)
                         .OrderBy(card => card.Order)
                         .Select(card => new SimpleKanbanCardResponse(
                             card.Id,
                             card.Title,
                             card.Description,
                             card.Order,
                             card.DueDate,
                             card.IsArchived
                         ))
                 )),
                c.Labels
                 .Where(l => !l.IsDeleted)
                 .Select(l => new KanbanLabelResponse(
                     l.Id,
                     l.Name,
                     l.ColorHex
                 )),
                c.Members
                 .Where(m => !m.IsDeleted)
                 .Select(m => new KanbanBoardMemberResponse(
                     m.Id,
                     m.UserId,
                     (int)m.Role
                 )),
                c.Tasks
                 .Where(t => !t.IsDeleted)
                 .Select(t => new BoardTaskResponse(
                     t.Id,
                     t.Title,
                     t.Description,
                     (int)t.Status,
                     (int)t.Priority,
                     t.DueDate,
                     t.AssigneeId,
                     t.Position
                 ))
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanBoardResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var board = await _context.KanbanBoards
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new KanbanBoardResponse(
                c.Id,
                c.Name,                   
                c.Description,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Columns
                 .Where(col => !col.IsDeleted)
                 .OrderBy(col => col.Order)
                 .Select(col => new KanbanColumnResponse(
                     col.Id,
                     col.Name,
                     col.Order,
                     col.IsArchived,
                     col.Cards
                         .Where(card => !card.IsDeleted)
                         .OrderBy(card => card.Order)
                         .Select(card => new SimpleKanbanCardResponse(
                             card.Id,
                             card.Title,
                             card.Description,
                             card.Order,
                             card.DueDate,
                             card.IsArchived
                         ))
                 )),
                c.Labels
                 .Where(l => !l.IsDeleted)
                 .Select(l => new KanbanLabelResponse(
                     l.Id,
                     l.Name,
                     l.ColorHex
                 )),
                c.Members
                 .Where(m => !m.IsDeleted)
                 .Select(m => new KanbanBoardMemberResponse(
                     m.Id,
                     m.UserId,
                     (int)m.Role
                 )),
                c.Tasks
                 .Where(t => !t.IsDeleted)
                 .Select(t => new BoardTaskResponse(
                     t.Id,
                     t.Title,
                     t.Description,
                     (int)t.Status,
                     (int)t.Priority,
                     t.DueDate,
                     t.AssigneeId,
                     t.Position
                 ))
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (board is null)
            return Result.Failure<KanbanBoardResponse>(_kanbanBoardErrors.KanbanBoardNotFound);

        return Result.Success(board);
    }

    public async Task<Result<KanbanBoardResponse>> AddAsync(KanbanBoardRequest request, CancellationToken cancellationToken = default)
    {
        var newBoard = _mapper.Map<KanbanBoard>(request);

        await _context.KanbanBoards.AddAsync(newBoard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<KanbanBoardResponse>(newBoard);

        return Result.Success(response);
    }

    public async Task<Result<KanbanBoardResponse>> UpdateAsync(KanbanBoardRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanBoards
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanBoardResponse>(_kanbanBoardErrors.KanbanBoardNotFound);

        var updated = _mapper.Map<KanbanBoard>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanBoards
                                     .Where(c => c.Id == updated.Id)
                                     .Select(c => _mapper.Map<KanbanBoardResponse>(c))
                                     .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var board = await _context.KanbanBoards
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (board == null)
            return Result.Failure(_kanbanBoardErrors.KanbanBoardNotFound);

        // Prevent deletion if board has non-deleted columns
        var hasActiveColumns = await _context.KanbanColumns.AnyAsync(col => col.KanbanBoardId == id && !col.IsDeleted, cancellationToken);
        if (hasActiveColumns)
            return Result.Failure(_kanbanBoardErrors.KanbanBoardHasColumns);

        board.IsDeleted = !board.IsDeleted;
        board.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        board.DeletedOn = DateTime.UtcNow;
        board.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}