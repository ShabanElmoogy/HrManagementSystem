using HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

namespace HrManagementSystem.Services.BasicServices.KanbanCardLabelsService;

public class KanbanCardLabelService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanCardLabelErrors kanbanCardLabelErrors) : IKanbanCardLabelService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanCardLabelErrors _kanbanCardLabelErrors = kanbanCardLabelErrors;

    public async Task<IEnumerable<KanbanCardLabelResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardLabels
            .AsNoTracking()
            .Where(cl => !cl.IsDeleted)
            .Select(cl => new KanbanCardLabelResponse(
                cl.Id,
                cl.KanbanCardId,
                cl.KanbanLabelId,
                cl.KanbanLabel.Name,
                cl.KanbanLabel.ColorHex,
                cl.CreatedOn,
                cl.UpdatedOn,
                cl.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardLabelResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var labelLink = await _context.KanbanCardLabels
            .AsNoTracking()
            .Where(cl => cl.Id == id && !cl.IsDeleted)
            .Select(cl => new KanbanCardLabelResponse(
                cl.Id,
                cl.KanbanCardId,
                cl.KanbanLabelId,
                cl.KanbanLabel.Name,
                cl.KanbanLabel.ColorHex,
                cl.CreatedOn,
                cl.UpdatedOn,
                cl.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (labelLink is null)
            return Result.Failure<KanbanCardLabelResponse>(_kanbanCardLabelErrors.KanbanCardLabelNotFound);

        return Result.Success(labelLink);
    }

    public async Task<IEnumerable<KanbanCardLabelResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardLabels
            .AsNoTracking()
            .Where(cl => cl.KanbanCardId == cardId && !cl.IsDeleted)
            .Select(cl => new KanbanCardLabelResponse(
                cl.Id,
                cl.KanbanCardId,
                cl.KanbanLabelId,
                cl.KanbanLabel.Name,
                cl.KanbanLabel.ColorHex,
                cl.CreatedOn,
                cl.UpdatedOn,
                cl.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardLabelResponse>> AddAsync(KanbanCardLabelRequest request, CancellationToken cancellationToken = default)
    {
        var newLink = _mapper.Map<KanbanCardLabel>(request);

        await _context.KanbanCardLabels.AddAsync(newLink, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardLabels
            .AsNoTracking()
            .Where(cl => cl.Id == newLink.Id)
            .Select(cl => new KanbanCardLabelResponse(
                cl.Id,
                cl.KanbanCardId,
                cl.KanbanLabelId,
                cl.KanbanLabel.Name,
                cl.KanbanLabel.ColorHex,
                cl.CreatedOn,
                cl.UpdatedOn,
                cl.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanCardLabelResponse>> UpdateAsync(KanbanCardLabelRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanCardLabels
            .FirstOrDefaultAsync(cl => cl.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanCardLabelResponse>(_kanbanCardLabelErrors.KanbanCardLabelNotFound);

        var updated = _mapper.Map<KanbanCardLabel>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardLabels
            .AsNoTracking()
            .Where(cl => cl.Id == updated.Id)
            .Select(cl => new KanbanCardLabelResponse(
                cl.Id,
                cl.KanbanCardId,
                cl.KanbanLabelId,
                cl.KanbanLabel.Name,
                cl.KanbanLabel.ColorHex,
                cl.CreatedOn,
                cl.UpdatedOn,
                cl.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var labelLink = await _context.KanbanCardLabels
            .FirstOrDefaultAsync(cl => cl.Id == id, cancellationToken);

        if (labelLink == null)
            return Result.Failure(_kanbanCardLabelErrors.KanbanCardLabelNotFound);

        labelLink.IsDeleted = !labelLink.IsDeleted;
        labelLink.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        labelLink.DeletedOn = DateTime.UtcNow;
        labelLink.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}