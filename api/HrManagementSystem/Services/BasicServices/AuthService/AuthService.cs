using HrManagementSystem.Contracts.BasicContracts.Authentication.Auth;
using HrManagementSystem.Hubs.GeneralHub;

namespace HrManagementSystem.Services.BasicServices.AuthService
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        UserErrors userErrors,
        ILogger<AuthService> logger,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender,
        IHubContext<GeneralHub, IGeneralHubClient> companyHubContext,
        ApplicationDbContext context) : IAuthService
    {
        public UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly UserErrors _userErrors = userErrors;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly ApplicationDbContext _context = context;
        private readonly int _refreshTokenExpireInDays = 14;
        private readonly IHubContext<GeneralHub, IGeneralHubClient> _companyHubContext = companyHubContext;

        #region "LoginAndRegister"

        public async Task<Result<AuthResponse>> LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal, CancellationToken cancellationToken = default)
        {
            if (claimsPrincipal == null)
            {
                return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
            }
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName) ?? "First Name",
                    LastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname) ?? "Last Name",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(newUser, "P@ssword123");
                if (!result.Succeeded)
                {
                    return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
                }
                await _userManager.AddToRoleAsync(newUser, AppRoles.user);
                if (!result.Succeeded)
                {
                    return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
                }
                user = newUser;
            }
            // Check if user is disabled
            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(_userErrors.DisabledUser);

            // Get Google identifier
            var providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            // Check if the external login exists
            var existingLogins = await _userManager.GetLoginsAsync(user);
            var googleLogin = existingLogins.FirstOrDefault(l =>
                l.LoginProvider == "Google" && l.ProviderKey == providerKey);

            // Only add the login if it doesn't exist
            if (googleLogin == null)
            {
                var info = new UserLoginInfo("Google", providerKey, "Google");
                var loginResult = await _userManager.AddLoginAsync(user, info);
                if (!loginResult.Succeeded)
                {
                    return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
                }
            }

            // Rest of your method remains the same
            // Create login record if needed (similar to GetTokenAsync)
            var userLogin = await CreateLoginAsync(user.Id, cancellationToken);
            if (!userLogin.IsSuccess)
                return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);
            // Generate JWT token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            // Handle refresh token - check if active one exists or create new
            if (user.RefreshTokens.Any(x => x.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                var refreshToken = activeRefreshToken!.Token;
                var refreshTokenExpiration = activeRefreshToken.ExpiresOn;
                var response = new AuthResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    token,
                    DateTime.UtcNow.AddMinutes(expiresIn),
                    refreshToken,
                    refreshTokenExpiration);
                return Result.Success(response);
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireInDays);
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });
                await _userManager.UpdateAsync(user);
                var response = new AuthResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    token,
                    DateTime.UtcNow.AddMinutes(expiresIn),
                    refreshToken,
                    refreshTokenExpiration);
                return Result.Success(response);
            }
        }
        public async Task<Result<AuthResponse>> GetTokenAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName) ?? await _userManager.FindByEmailAsync(userName);
            if (user is null)
                return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(_userErrors.DisabledUser);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

            if (result.Succeeded)
            {
                var userLogin = await CreateLoginAsync(user.Id, cancellationToken);
                if (!userLogin.IsSuccess)
                    return Result.Failure<AuthResponse>(_userErrors.InvalidCredentials);

                var (token, expiresIn) = _jwtProvider.GenerateToken(user);

                if (user.RefreshTokens.Any(x => x.IsActive))
                {
                    var activerefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                    var refreshToken = activerefreshToken!.Token;
                    var refreshTokenExpiration = activerefreshToken.ExpiresOn;

                    var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, DateTime.UtcNow.AddMinutes(expiresIn), refreshToken, refreshTokenExpiration);
                    return Result.Success(response);
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireInDays);

                    user.RefreshTokens.Add(new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiresOn = refreshTokenExpiration
                    });

                    await _userManager.UpdateAsync(user);

                    var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, DateTime.UtcNow.AddMinutes(expiresIn), refreshToken, refreshTokenExpiration);
                    return Result.Success(response);
                }
            }

            var error = result.IsNotAllowed ? _userErrors.EmailNotConfirmed :
                        result.IsLockedOut ? _userErrors.LockedUser : _userErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            // Validate presence of both tokens
            if (string.IsNullOrEmpty(refreshTokenRequest.Token) || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
                return Result.Failure<AuthResponse>(_userErrors.InvalidJwtToken);

            // Validate JWT token and get user ID
            var userId = _jwtProvider.ValidateToken(refreshTokenRequest.Token);
            if (userId is null)
                return Result.Failure<AuthResponse>(_userErrors.InvalidJwtToken);

            // Find user
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(_userErrors.InvalidJwtToken);

            // Check user status
            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(_userErrors.DisabledUser);

            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(_userErrors.LockedUser);

            // Validate refresh token
            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenRequest.RefreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(_userErrors.InvalidRefreshToken);

            string newRefreshToken;
            DateTime refreshTokenExpiration;

            // Check if current refresh token is expired
            if (userRefreshToken.IsExpired)
            {
                // Mark the current token as revoked
                userRefreshToken.RevokedOn = DateTime.UtcNow;

                // Generate new refresh token
                newRefreshToken = GenerateRefreshToken();
                refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireInDays);

                // Add the new token to user's refresh tokens
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = newRefreshToken,
                    ExpiresOn = refreshTokenExpiration
                });
            }
            else
            {
                // Use existing refresh token if not expired
                newRefreshToken = userRefreshToken.Token;
                refreshTokenExpiration = userRefreshToken.ExpiresOn;
            }

            // Generate new JWT token
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
            var tokenExpiration = DateTime.UtcNow.AddMinutes(expiresIn);

            // Save changes to user
            await _userManager.UpdateAsync(user);

            // Create and return response
            var response = new AuthResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                newToken,
                tokenExpiration,
                newRefreshToken,
                refreshTokenExpiration);

            return Result.Success(response);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users
                        .Include(x => x.RefreshTokens)
                        .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user is null)
                return Result.Failure(_userErrors.InvalidRefreshToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(_userErrors.InvalidRefreshToken);

            // Mark the token as revoked
            userRefreshToken.RevokedOn = DateTime.UtcNow;

            // Save changes to database
            await _userManager.UpdateAsync(user);

            var x = _companyHubContext.Clients.User(user.Id);
            // Send SignalR notification with proper await
            await _companyHubContext.Clients.All.ReceiveTokenRevoked("Your session has been revoked by an administrator");

            return Result.Success();
        }

        public async Task<Result> RevokeRefreshTokenByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                        .Include(x => x.RefreshTokens)
                        .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user is null)
                return Result.Failure(_userErrors.UserNotFound);

            // Get all active refresh tokens for the user
            var activeRefreshTokens = user.RefreshTokens.Where(x => x.IsActive).ToList();

            if (!activeRefreshTokens.Any())
                return Result.Failure(_userErrors.NoActiveRefreshTokens);

            // Mark all active tokens as revoked
            foreach (var token in activeRefreshTokens)
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            // Save changes to database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Failure(_userErrors.UpdateFailed);

            // Send SignalR notification to the user
            await _companyHubContext.Clients.User(user.Id).ReceiveTokenRevoked("Your session has been revoked by an administrator");

            return Result.Success();
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure(_userErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confirmation code: {code}", code);

                await SendConfirmationEmail(user, code);

                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> LogOut(string userId, CancellationToken cancellationToken)
        {
            if (userId is null)
                return Result.Failure(_userErrors.InvalidCredentials);

            var currentLogin = await _context.UserLogins
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            if (currentLogin is not null)
                currentLogin.LogOutDate = DateTime.UtcNow;
            else
                return Result.Success();

            _context.Update(currentLogin);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> CreateLoginAsync(string userId, CancellationToken cancellationToken)
        {
            if (userId is null)
                return Result.Failure(_userErrors.InvalidCredentials);

            var userLogin = new UserLogin
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                LoginDate = DateTime.UtcNow
            };

            _context.Add(userLogin);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        #endregion

        #region "RefreshToken"
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).TrimEnd('=');
        }

        #endregion

        #region "Confirm Email"

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(_userErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(_userErrors.DuplicatedConfirmation);

            var code = request.Code;

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(_userErrors.InvalidCode);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppRoles.user);
                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(_userErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var returnUrl = _httpContextAccessor.HttpContext?.Request.Headers["ReturnUrl"].ToString();

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "auth/emailConfirmation";
            }

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                        { "{{action_url}}", $"{origin}/{returnUrl}?userId={user.Id}&code={code}" }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody));

            await Task.CompletedTask;
        }

        #endregion

        #region "Reset Password"

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(_userErrors.InvalidCode);

            IdentityResult result;

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();

            if (!user.EmailConfirmed)
                return Result.Failure(_userErrors.EmailNotConfirmed);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Reset code: {code}", code);

            await SendResetPasswordEmail(user, code);

            return Result.Success();
        }

        private async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var returnUrl = _httpContextAccessor.HttpContext?.Request.Headers["ReturnUrl"].ToString();

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "auth/forgetPassword";
            }

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/{returnUrl}?email={user.Email}&code={code}" }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ TechnicalSupportApp: Change Password", emailBody));

            await Task.CompletedTask;
        }

        #endregion
    }
}
