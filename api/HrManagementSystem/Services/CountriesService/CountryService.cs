using EFCore.BulkExtensions;
using HrManagementSystem.Hubs.GeneralHub;

namespace HrManagementSystem.Services.CountriesService;

public class CountryService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IEntityChangeLogService entityChangeLogService,
    CountryErrors countryErrors,
    IHubContext<GeneralHub, IGeneralHubClient> generalHubContext,
    IMapper mapper) : ICountryService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEntityChangeLogService _entityChangeLogService = entityChangeLogService;
    private readonly CountryErrors _countryErrors = countryErrors;
    private readonly IHubContext<GeneralHub, IGeneralHubClient> _generalHubContext = generalHubContext;

    public async Task<IEnumerable<CountryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var countries =  await _context.Countries
                                     .AsNoTracking()
                                     .ProjectToType<CountryResponse>()
                                     .ToListAsync(cancellationToken);
            
        return countries;
    }

    public async Task<Result<CountryResponse>>? GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _context.Countries.FindAsync(id, cancellationToken);

        return response is not null
        ? Result.Success(response.Adapt<CountryResponse>())
            : Result.Failure<CountryResponse>(_countryErrors.CountryNotFound);
    }

    public async Task<Result<CountryResponse>>? GetRelatedStates(int id, CancellationToken cancellationToken = default)
    {
        var response = await _context.Countries
                                        .Include(c => c.States)
                                        .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return response is null
             ? Result.Failure<CountryResponse>(_countryErrors.CountryNotFound)
             : Result.Success(response.Adapt<CountryResponse>());
    }

    public async Task<Result<CountryResponse>> AddAsync(CountryRequest countryRequest, CancellationToken cancellationToken = default)
    {
        var newCountry = _mapper.Map<Country>(countryRequest);

        await _context.AddAsync(newCountry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = newCountry.Adapt<CountryResponse>();

        var countriesCount = await GetCountAsync(cancellationToken);

        await _generalHubContext.Clients.All.ReceiveCountryUpdate(new CountriesCountResponse(countriesCount.Value.Count,response,"Add"));

        return Result.Success(response);
    }

    public async Task<Result> AddRangeAsync(List<CountryRequest> countryRequests, CancellationToken cancellationToken = default)
    {
        if (countryRequests == null || countryRequests.Count == 0)
            return Result.Failure(_countryErrors.NoCountriesProvided);

        var duplicateNames = countryRequests
            .GroupBy(x => x.NameAr)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateNames.Count != 0)
        {
            return Result.Failure(_countryErrors.CountryExists);
        }

        var newCountries = new List<Country>();
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId()!;

        foreach (var countryRequest in countryRequests)
        {
            // Check if NameAr or NameEn exists in the database
            var nameExists = await _context.Countries.AnyAsync(
                c => c.NameAr == countryRequest.NameAr || c.NameEn == countryRequest.NameEn,
                cancellationToken
            );

            if (nameExists)
                return Result.Failure(_countryErrors.CountryExists);

            // Map the request to a new Country entity
            var newCountry = _mapper.Map<Country>(countryRequest);
            newCountry.CreatedById = currentUserId;
            newCountries.Add(newCountry);
        }

        // Add all countries to the database at once using bulk insert
        await _context.BulkInsertAsync(newCountries, cancellationToken: cancellationToken);
        return Result.Success();
    }

    public async Task<Result<CountryResponse>> UpdateAsync(CountryRequest countryRequest, CancellationToken cancellationToken = default)
    {
        var currentCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryRequest.Id, cancellationToken);

        if (currentCountry is null)
            return Result.Failure<CountryResponse>(_countryErrors.CountryNotFound);

        var updatedCountry = countryRequest.Adapt<Country>();
        await _entityChangeLogService.CreateChangeLogAsync(countryRequest.Id, currentCountry, updatedCountry);

        mapper.Map(countryRequest, currentCountry);
        _context.Update(currentCountry);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<CountryResponse>(currentCountry);

        return Result.Success(response);
    }

    public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        var country = await _context.Countries.FindAsync(id);

        if (country is null)
            return Result.Failure(_countryErrors.CountryNotFound);

        var isInState = await _context.States.AnyAsync(s => s.CountryId == id, cancellationToken);
        if (isInState)
            return Result.Failure(_countryErrors.CountryInUseByState);

        country.IsDeleted = !country.IsDeleted;
        country.DeletedById = _httpContextAccessor.HttpContext!.User.GetUserId()!;
        country.DeletedByPc = Environment.MachineName;
        country.DeletedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<CountriesCountResponse>> GetCountAsync(CancellationToken cancellationToken = default)
    {
        var count = await _context.Countries
                          .Where(c => !c.IsDeleted)
                          .CountAsync(cancellationToken: cancellationToken);

        var response = new CountriesCountResponse(count,null,null);

        return Result.Success(response);
    }
}