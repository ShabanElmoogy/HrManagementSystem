using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace HrManagementSystem.Controllers.V1;

[Route("api/account")]
[ApiController]
public class GoogleAuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly LinkGenerator _linkGenerator;
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration; // Add this field

    public GoogleAuthController(
        SignInManager<ApplicationUser> signInManager,
        LinkGenerator linkGenerator,
        IAuthService authService,
        IConfiguration configuration) // Add this parameter
    {
        _signInManager = signInManager;
        _linkGenerator = linkGenerator;
        _authService = authService;
        _configuration = configuration; // Initialize the field
    }

    //Server Side Solution

    [HttpGet("login/google")]
    public IActionResult GoogleLogin([FromQuery] string returnUrl)
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            "Google",
            _linkGenerator.GetPathByName(HttpContext, "GoogleLoginCallback")
                + $"?returnUrl={returnUrl}");
        return Challenge(properties, new[] { "Google" });
    }

    [HttpGet("login/google/callback")]
    [ActionName("GoogleLoginCallback")] // This makes the name available for GetPathByName
    public async Task<IActionResult> GoogleLoginCallback([FromQuery] string returnUrl)
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }
        await _authService.LoginWithGoogleAsync(result.Principal);
        return Redirect(returnUrl);
    }


    // Client Side Send Token 
    [HttpPost("google-auth")]
    public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Credential))
            return BadRequest(new { error = "Missing credentials" });

        try
        {
            // This is specifically for handling access tokens (ya29.*)
            // These tokens must be sent to Google's userinfo endpoint
            using (var httpClient = new HttpClient())
            {
                // Set proper Authorization header with the token
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.Credential);

                // Call Google's userinfo endpoint
                var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest(new
                    {
                        error = "Invalid access token",
                        details = $"Google API returned {response.StatusCode}: {errorContent}"
                    });
                }

                // Read and log the response
                var userInfoJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Google user info: {userInfoJson}");

                // Configure the JSON deserializer properly
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialize the user info
                var userInfo = System.Text.Json.JsonSerializer.Deserialize<GoogleUserInfo>(userInfoJson, options);

                if (userInfo == null)
                {
                    return BadRequest(new { error = "Failed to parse user info" });
                }

                // Create claims from the user info
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Sub),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypes.GivenName, userInfo.GivenName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, userInfo.FamilyName ?? string.Empty)
                };

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Google"));

                // Call your auth service
                var result = await _authService.LoginWithGoogleAsync(principal);
                if (!result.IsSuccess)
                    return BadRequest(new { error = result.Error });

                return Ok(result.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Google auth exception: {ex}");
            return BadRequest(new { error = "Authentication failed", details = ex.Message });
        }
    }

}

// Add these classes
public class GoogleAuthRequest
{
    public string Credential { get; set; }
}

public class GoogleUserInfo
{
    public string Sub { get; set; }
    public string Name { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Picture { get; set; }
    public string Email { get; set; }
    public bool EmailVerified { get; set; }
    public string Locale { get; set; }
}