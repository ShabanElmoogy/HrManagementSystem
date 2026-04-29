using HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

namespace HrManagementSystem.Services.BasicServices.KanbanCardAssigneesService;

public class KanbanCardAssigneeService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanCardAssigneeErrors kanbanCardAssigneeErrors) : IKanbanCardAssigneeService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanCardAssigneeErrors _kanbanCardAssigneeErrors = kanbanCardAssigneeErrors;

    public async Task<IEnumerable<KanbanCardAssigneeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanCardAssignees
            .AsNoTracking()
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
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardAssigneeResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var assignee = await _context.KanbanCardAssignees
            .AsNoTracking()
            .Where(a => a.Id == id && !a.IsDeleted)
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (assignee is null)
            return Result.Failure<KanbanCardAssigneeResponse>(_kanbanCardAssigneeErrors.KanbanCardAssigneeNotFound);

        return Result.Success(assignee);
    }

    public async Task<IEnumerable<KanbanCardAssigneeResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken)
    {
        return await _context.KanbanCardAssignees
            .AsNoTracking()
            .Where(a => a.KanbanCardId == cardId && !a.IsDeleted)
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
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanCardAssigneeResponse>> AddAsync(KanbanCardAssigneeRequest request, CancellationToken cancellationToken = default)
    {
        var newAssignee = _mapper.Map<KanbanCardAssignee>(request);

        await _context.KanbanCardAssignees.AddAsync(newAssignee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardAssignees
            .AsNoTracking()
            .Where(a => a.Id == newAssignee.Id)
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanCardAssigneeResponse>> UpdateAsync(KanbanCardAssigneeRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanCardAssignees
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanCardAssigneeResponse>(_kanbanCardAssigneeErrors.KanbanCardAssigneeNotFound);

        var updated = _mapper.Map<KanbanCardAssignee>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanCardAssignees
            .AsNoTracking()
            .Where(a => a.Id == updated.Id)
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var assignee = await _context.KanbanCardAssignees
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (assignee == null)
            return Result.Failure(_kanbanCardAssigneeErrors.KanbanCardAssigneeNotFound);

        assignee.IsDeleted = !assignee.IsDeleted;
        assignee.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        assignee.DeletedOn = DateTime.UtcNow;
        assignee.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
