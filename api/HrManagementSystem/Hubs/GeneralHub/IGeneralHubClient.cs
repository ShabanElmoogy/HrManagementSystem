namespace HrManagementSystem.Hubs.GeneralHub;

public interface IGeneralHubClient
{
    Task ReceiveUserUpdate(Result<UsersCountResponse> usersCount);
    Task ReceiveCountryUpdate(CountriesCountResponse countriesCount);
    Task ReceiveStateUpdate(Result<StatesCountResponse> statesCount);
    Task ReceiveDistrictUpdate(Result<DistrictsCountResponse> districtsCount);
    Task ReceiveAddressTypeUpdate(Result<AddressTypesCountResponse> addressTypesCount);
    Task ReceiveAddressUpdate(Result<AddressesCountResponse> addressesCount);
    Task ReceiveTokenRevoked(string message); 
}
