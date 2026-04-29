using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Authentication
{
    public interface IJwtProvider
    {
        (string token, long expiresIn) GenerateToken(ApplicationUser user);

        string? ValidateToken(string token);
    }
}
