namespace HrManagementSystem.Services.CountriesService;
public interface ICountryService
{
    Task<IEnumerable<CountryResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<CountryResponse>>? GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<CountryResponse>>? GetRelatedStates(int id, CancellationToken cancellationToken);
    Task<Result<CountryResponse>> AddAsync(CountryRequest country, CancellationToken cancellationToken);
    Task<Result> AddRangeAsync(List<CountryRequest> countryRequests, CancellationToken cancellationToken = default);
    Task<Result<CountryResponse>> UpdateAsync(CountryRequest countryRequest, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
    Task<Result<CountriesCountResponse>> GetCountAsync(CancellationToken cancellationToken = default);
}
