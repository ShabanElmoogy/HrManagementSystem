namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]

public class AuthController(IAuthService authService) : ControllerBase
{
    public readonly IAuthService _authService = authService;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.UserName, request.Password, cancellationToken);

        return authResult.IsSuccess
                      ? Ok(authResult.Value)
                      : authResult.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> LogOut([FromQuery] string userId, CancellationToken cancellationToken)
    {
        var result = await _authService.LogOut(userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(refreshTokenRequest, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPut]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(refreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut]
    public async Task<IActionResult> RevokeRefreshTokenByUserId(string userId, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenByUserIdAsync(userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ResetPasswordAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("CheckAuth")]
    [Authorize] // Ensures only authenticated users can access
    public IActionResult CheckAuth()
    {
        return Ok(new { isAuthenticated = true });
    }
}
