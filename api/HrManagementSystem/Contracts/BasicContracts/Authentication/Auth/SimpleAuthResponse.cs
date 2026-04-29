namespace HrManagementSystem.Contracts.BasicContracts.Authentication.Auth
{
    public record SimpleAuthResponse(
        string UserName,
        string Password
        );
}
