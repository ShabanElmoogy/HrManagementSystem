using HrManagementSystem.Contracts.BasicContracts.Authentication.Auth;

namespace HrManagementSystem.Services.BasicServices.AuthService;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string userName, string password, CancellationToken cancellationToken);

    Task<Result> CreateLoginAsync(string userId, CancellationToken cancellationToken);

    Task<Result<AuthResponse>> LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal, CancellationToken cancellationToken = default);

    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<Result> LogOut(string userId, CancellationToken cancellationToken);

    Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);

    Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<Result> RevokeRefreshTokenByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken);

    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken);

    Task<Result> SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken);

    Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);

}
