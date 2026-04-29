namespace HrManagementSystem.Contracts.BasicContracts.Authentication.Auth
{
    public record AuthResponse(
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string Token,
        DateTime TokenExpiration,
        string RefreshToken,
        DateTime RefreshTokenExpiration
        );
}
