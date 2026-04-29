using HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers;

namespace HrManagementSystem.Services.BasicServices.KanbanBoardMembersService;

public class KanbanBoardMemberService(
    IMapper mapper,
    ApplicationDbContext context,
    IEntityChangeLogService entityChangeLogService,
    IHttpContextAccessor httpContextAccessor,
    KanbanBoardMemberErrors kanbanBoardMemberErrors) : IKanbanBoardMemberService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _context = context;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly KanbanBoardMemberErrors _kanbanBoardMemberErrors = kanbanBoardMemberErrors;

    public async Task<IEnumerable<KanbanBoardMemberResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KanbanBoardMembers
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .Include(m => m.KanbanBoard)
            .Include(m => m.User)
            .Select(m => new KanbanBoardMemberResponse(
                m.Id,
                m.KanbanBoardId,
                m.KanbanBoard.Name,
                m.UserId,
                m.User.UserName ?? string.Empty,
                m.User.Email ?? string.Empty,
                (int)m.Role,
                m.Role.ToString(),
                m.CreatedOn,
                m.UpdatedOn,
                m.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanBoardMemberResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var member = await _context.KanbanBoardMembers
            .AsNoTracking()
            .Where(m => m.Id == id && !m.IsDeleted)
            .Include(m => m.KanbanBoard)
            .Include(m => m.User)
            .Select(m => new KanbanBoardMemberResponse(
                m.Id,
                m.KanbanBoardId,
                m.KanbanBoard.Name,
                m.UserId,
                m.User.UserName ?? string.Empty,
                m.User.Email ?? string.Empty,
                (int)m.Role,
                m.Role.ToString(),
                m.CreatedOn,
                m.UpdatedOn,
                m.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (member is null)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardMemberNotFound);

        return Result.Success(member);
    }

    public async Task<IEnumerable<KanbanBoardMemberResponse>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken)
    {
        return await _context.KanbanBoardMembers
            .AsNoTracking()
            .Where(m => m.KanbanBoardId == boardId && !m.IsDeleted)
            .Include(m => m.KanbanBoard)
            .Include(m => m.User)
            .Select(m => new KanbanBoardMemberResponse(
                m.Id,
                m.KanbanBoardId,
                m.KanbanBoard.Name,
                m.UserId,
                m.User.UserName ?? string.Empty,
                m.User.Email ?? string.Empty,
                (int)m.Role,
                m.Role.ToString(),
                m.CreatedOn,
                m.UpdatedOn,
                m.IsDeleted
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<KanbanBoardMemberResponse>> AddAsync(KanbanBoardMemberRequest request, CancellationToken cancellationToken = default)
    {
        // Verify board exists
        var boardExists = await _context.KanbanBoards
            .AnyAsync(b => b.Id == request.KanbanBoardId && !b.IsDeleted, cancellationToken);

        if (!boardExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardNotFound);

        // Verify user exists
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId && !u.IsDisabled, cancellationToken);

        if (!userExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.UserNotFound);

        // Check for duplicate membership
        var membershipExists = await _context.KanbanBoardMembers
            .AnyAsync(m => m.KanbanBoardId == request.KanbanBoardId && 
                          m.UserId == request.UserId && 
                          !m.IsDeleted, cancellationToken);

        if (membershipExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardMemberExists);

        var newMember = _mapper.Map<KanbanBoardMember>(request);

        await _context.KanbanBoardMembers.AddAsync(newMember, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanBoardMembers
            .AsNoTracking()
            .Where(m => m.Id == newMember.Id)
            .Include(m => m.KanbanBoard)
            .Include(m => m.User)
            .Select(m => new KanbanBoardMemberResponse(
                m.Id,
                m.KanbanBoardId,
                m.KanbanBoard.Name,
                m.UserId,
                m.User.UserName ?? string.Empty,
                m.User.Email ?? string.Empty,
                (int)m.Role,
                m.Role.ToString(),
                m.CreatedOn,
                m.UpdatedOn,
                m.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result<KanbanBoardMemberResponse>> UpdateAsync(KanbanBoardMemberRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _context.KanbanBoardMembers
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (current == null)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardMemberNotFound);

        // Verify board exists
        var boardExists = await _context.KanbanBoards
            .AnyAsync(b => b.Id == request.KanbanBoardId && !b.IsDeleted, cancellationToken);

        if (!boardExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardNotFound);

        // Verify user exists
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId && !u.IsDisabled, cancellationToken);

        if (!userExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.UserNotFound);

        // Check for duplicate membership (excluding current record)
        var membershipExists = await _context.KanbanBoardMembers
            .AnyAsync(m => m.KanbanBoardId == request.KanbanBoardId && 
                          m.UserId == request.UserId && 
                          m.Id != request.Id &&
                          !m.IsDeleted, cancellationToken);

        if (membershipExists)
            return Result.Failure<KanbanBoardMemberResponse>(_kanbanBoardMemberErrors.KanbanBoardMemberExists);

        var updated = _mapper.Map<KanbanBoardMember>(request);
        await _entityChangeLogService.CreateChangeLogAsync(request.Id, current, updated);

        _mapper.Map(request, current);

        _context.Update(current);
        await _context.SaveChangesAsync(cancellationToken);

        var response = await _context.KanbanBoardMembers
            .AsNoTracking()
            .Where(m => m.Id == current.Id)
            .Include(m => m.KanbanBoard)
            .Include(m => m.User)
            .Select(m => new KanbanBoardMemberResponse(
                m.Id,
                m.KanbanBoardId,
                m.KanbanBoard.Name,
                m.UserId,
                m.User.UserName ?? string.Empty,
                m.User.Email ?? string.Empty,
                (int)m.Role,
                m.Role.ToString(),
                m.CreatedOn,
                m.UpdatedOn,
                m.IsDeleted
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(response!);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _context.KanbanBoardMembers
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (member == null)
            return Result.Failure(_kanbanBoardMemberErrors.KanbanBoardMemberNotFound);

        member.IsDeleted = !member.IsDeleted;
        member.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId();
        member.DeletedOn = DateTime.UtcNow;
        member.DeletedByPc = Environment.MachineName;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
