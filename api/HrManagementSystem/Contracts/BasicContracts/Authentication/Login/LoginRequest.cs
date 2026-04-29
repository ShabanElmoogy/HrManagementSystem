namespace HrManagementSystem.Contracts.BasicContracts.Authentication.Login
{
    public record LoginRequest(
        string UserName,
        string Password
        );
}