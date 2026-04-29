using HrManagementSystem.Contracts.BasicContracts.KanbanLabels;

namespace HrManagementSystem.Services.BasicServices.KanbanLabelsService;

public class KanbanLabelService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanLabelErrors kanbanLabelErrors) : IKanbanLabelService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanLabelErrors _kanbanLabelErrors = kanbanLabelErrors;

    public async Task<IEnumerable<KanbanLabelResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanLabels
            .AsNoTracking()
            .Where(l => !l.IsDeleted)
            .Select(l => new KanbanLabelResponse(
                l.Id,
                l.KanbanBoardId,
                l.Name,
                l.ColorHex,
                l.CreatedOn,
                l.UpdatedOn,
                l.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanLabelResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var label = await _context.KanbanLabels
            .AsNoTracking()
            .Where(l => l.Id == id && !l.IsDeleted)
            .Select(l => new KanbanLabelResponse(
                l.Id,
                l.KanbanBoardId,
                l.Name,
                l.ColorHex,
                l.CreatedOn,
                l.UpdatedOn,
                l.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (label is null)
            return Result.Failure<KanbanLabelResponse>(_kanbanLabelErrors.KanbanLabelNotFound);

        return Result.Success(label);
    }

    public async Task<Result<KanbanLabelResponse>> AddAsync(KanbanLabelRequest request, CancellationToken cancellationToken = default)
    {
        var newLabel = _mapper.Map<KanbanLabel>(request);

        await _context.KanbanLabels.AddAsync(newLabel, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanLabels
            .AsNoTracking()
            .Where(l => l.Id == newLabel.Id)
            .Select(l => new KanbanLabelResponse(
                l.Id,
                l.KanbanBoardId,
                l.Name,
                l.ColorHex,
                l.CreatedOn,
                l.UpdatedOn,
                l.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanLabelResponse>> UpdateAsync(KanbanLabelRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanLabels
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanLabelResponse>(_kanbanLabelErrors.KanbanLabelNotFound);

        var updated = _mapper.Map<KanbanLabel>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanLabels
                                     .Where(l => l.Id == updated.Id)
                                     .Select(l => new KanbanLabelResponse(
                                         l.Id,
                                         l.KanbanBoardId,
                                         l.Name,
                                         l.ColorHex,
                                         l.CreatedOn,
                                         l.UpdatedOn,
                                         l.IsDeleted
                                     ))
                                     .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var label = await _context.KanbanLabels
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (label == null)
            return Result.Failure(_kanbanLabelErrors.KanbanLabelNotFound);

        // Prevent deletion if label is linked to any non-deleted card labels
        var inUse = await _context.KanbanCardLabels
            .AnyAsync(cl => cl.KanbanLabelId == id && !cl.IsDeleted, cancellationToken);
        if (inUse)
            return Result.Failure(_kanbanLabelErrors.KanbanLabelInUse);

        label.IsDeleted = !label.IsDeleted;
        label.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        label.DeletedOn = DateTime.UtcNow;
        label.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}