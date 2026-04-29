using System.Text.Json;
using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Authentication
{
    public class JwtProvider(IOptions<JwtOptions> jwtSettings, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : IJwtProvider
    {
        private readonly JwtOptions _jwtSettings = jwtSettings.Value;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public (string token, long expiresIn) GenerateToken(ApplicationUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var expiresIn = _jwtSettings.ExpireInMintues;
            var claims = GetClaims(user).GetAwaiter().GetResult();
            var token = GetSecurityTokenOptions(signingCredentials, claims, expiresIn);

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            return new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            // Fetch user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Initialize claims list with basic user information
            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(MyClaims.firstname, user.FirstName),
            new(MyClaims.lastname, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray) //To Create New Token Every Time
        };

            foreach (var role in roles)
            {
                var applicationRole = await _roleManager.FindByNameAsync(role);
                if (applicationRole != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(applicationRole);
                    claims.AddRange(roleClaims);
                }
            }

            return claims;
        }

        private JwtSecurityToken GetSecurityTokenOptions(SigningCredentials signingCredentials, List<Claim> claims, long expiresIn)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer!,
                audience: _jwtSettings.Audience!,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresIn),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
