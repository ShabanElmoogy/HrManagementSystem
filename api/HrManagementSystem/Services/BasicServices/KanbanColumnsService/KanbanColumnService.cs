using HrManagementSystem.Contracts.BasicContracts.KanbanColumns;
using HrManagementSystem.Contracts.BasicContracts.KanbanCards;

namespace HrManagementSystem.Services.BasicServices.KanbanColumnsService;

public class KanbanColumnService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanColumnErrors kanbanColumnErrors) : IKanbanColumnService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanColumnErrors _kanbanColumnErrors = kanbanColumnErrors;

    public async Task<IEnumerable<KanbanColumnResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanColumns
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Order)
            .Select(c => new KanbanColumnResponse(
                c.Id,
                c.KanbanBoardId,
                c.Name,
                c.Order,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Cards
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
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanColumnResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var column = await _context.KanbanColumns
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new KanbanColumnResponse(
                c.Id,
                c.KanbanBoardId,
                c.Name,
                c.Order,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Cards
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (column is null)
            return Result.Failure<KanbanColumnResponse>(_kanbanColumnErrors.KanbanColumnNotFound);

        return Result.Success(column);
    }

    public async Task<Result<KanbanColumnResponse>> AddAsync(KanbanColumnRequest request, CancellationToken cancellationToken = default)
    {
        var newColumn = _mapper.Map<KanbanColumn>(request);

        await _context.KanbanColumns.AddAsync(newColumn, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanColumns
            .AsNoTracking()
            .Where(c => c.Id == newColumn.Id)
            .Select(c => new KanbanColumnResponse(
                c.Id,
                c.KanbanBoardId,
                c.Name,
                c.Order,
                c.IsArchived,
                c.CreatedOn,
                c.UpdatedOn,
                c.IsDeleted,
                c.Cards
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanColumnResponse>> UpdateAsync(KanbanColumnRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanColumns
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanColumnResponse>(_kanbanColumnErrors.KanbanColumnNotFound);

        var updated = _mapper.Map<KanbanColumn>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanColumns
                                     .Where(c => c.Id == updated.Id)
                                     .Select(c => new KanbanColumnResponse(
                                         c.Id,
                                         c.KanbanBoardId,
                                         c.Name,
                                         c.Order,
                                         c.IsArchived,
                                         c.CreatedOn,
                                         c.UpdatedOn,
                                         c.IsDeleted,
                                         c.Cards
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
                                     ))
                                     .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var column = await _context.KanbanColumns
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (column == null)
            return Result.Failure(_kanbanColumnErrors.KanbanColumnNotFound);

        // Prevent deletion if column has non-deleted cards
        var hasActiveCards = await _context.KanbanCards.AnyAsync(card => card.KanbanColumnId == id && !card.IsDeleted, cancellationToken);
        if (hasActiveCards)
            return Result.Failure(_kanbanColumnErrors.KanbanColumnHasCards);

        column.IsDeleted = !column.IsDeleted;
        column.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        column.DeletedOn = DateTime.UtcNow;
        column.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}