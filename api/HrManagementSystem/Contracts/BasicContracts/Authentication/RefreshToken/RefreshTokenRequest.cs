namespace HrManagementSystem.Contracts.BasicContracts.Authentication.RefreshToken
{
    public record RefreshTokenRequest(
        string? Token,
        string RefreshToken
        );


}
