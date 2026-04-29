using HrManagementSystem.Hubs.GeneralHub;
using Mapster;

namespace HrManagementSystem.Services.StateService;

public class StateService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IEntityChangeLogService entityChangeLogService,
    StateErrors stateErrors,
    IHubContext<GeneralHub, IGeneralHubClient> generalHubContext,
    IMapper mapper) : IStateService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly StateErrors _stateErrors = stateErrors;
    private readonly IHubContext<GeneralHub, IGeneralHubClient> _generalHubContext = generalHubContext;

    public async Task<IEnumerable<StateResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var states = await _context.States
                                   .AsNoTracking()
                                   .ProjectToType<StateResponse>()
                                   .ToListAsync(cancellationToken);

        return states;
    }

    public async Task<IEnumerable<StateResponse>> GetAllByCountryAsync(int countryId, CancellationToken cancellationToken = default)
    {
        var states = await _context.States
                                   .AsNoTracking()
                                   .Where(s => s.CountryId == countryId && !s.IsDeleted)
                                   .ProjectToType<StateResponse>()
                                   .ToListAsync(cancellationToken);

        return states;
    }

    public async Task<Result<StateResponse>>? GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _context.States.FindAsync(id, cancellationToken);

        return response is not null
        ? Result.Success(response.Adapt<StateResponse>())
            : Result.Failure<StateResponse>(_stateErrors.StateNotFound);
    }

    public async Task<Result<StateResponse>>? GetRelatedDistricts(int id, CancellationToken cancellationToken = default)
    {
        var response = await _context.States
                            .Include(s => s.Districts)
                            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return response is null
             ? Result.Failure<StateResponse>(_stateErrors.StateNotFound)
             : Result.Success(response.Adapt<StateResponse>());
    }

    public async Task<Result<StateResponse>> AddAsync(StateRequest stateRequest, CancellationToken cancellationToken = default)
    {
        var newState = _mapper.Map<State>(stateRequest);

        await _context.AddAsync(newState, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var savedState = await _context.States
                               .Include(c => c.Country)
                               .FirstOrDefaultAsync(s => s.Id == newState.Id, cancellationToken);

        var response = _mapper.Map<StateResponse>(savedState!);

        var statesCount = await GetCountAsync(cancellationToken);

        await _generalHubContext.Clients.All.ReceiveStateUpdate(statesCount);

        return Result.Success(response);
    }

    public async Task<Result<StateResponse>> UpdateAsync(StateRequest stateRequest, CancellationToken cancellationToken = default)
    {
        var currentState = await _context.States.FirstOrDefaultAsync(s => s.Id == stateRequest.Id, cancellationToken);

        if (currentState is null)
            return Result.Failure<StateResponse>(_stateErrors.StateNotFound);

        var updatedState = stateRequest.Adapt<State>();
        await _entityChangeLogService.CreateChangeLogAsync(stateRequest.Id, currentState, updatedState);

        _mapper.Map(stateRequest, currentState);
        _context.Update(currentState);
        await _context.SaveChangesAsync(cancellationToken);

        var savedState = await _context.States
                                       .Include(c => c.Country)
                                       .FirstOrDefaultAsync(s => s.Id == stateRequest.Id, cancellationToken);

        var response = _mapper.Map<StateResponse>(savedState);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var state = await _context.States.FindAsync(id);

        if (state is null)
            return Result.Failure(_stateErrors.StateNotFound);

        var isInDistrict = await _context.Districts.AnyAsync(d => d.StateId == id, cancellationToken);
        if (isInDistrict)
            return Result.Failure(_stateErrors.StateInUseByDistrict);

        state.IsDeleted = !state.IsDeleted;
        state.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId()!;
        state.DeletedByPc = Environment.MachineName;
        state.DeletedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<StatesCountResponse>> GetCountAsync(CancellationToken cancellationToken = default)
    {
        var count = await _context.States
                                  .Where(s => !s.IsDeleted)
                                  .CountAsync(cancellationToken);

        var response = new StatesCountResponse(count);

        return Result.Success(response);
    }
}