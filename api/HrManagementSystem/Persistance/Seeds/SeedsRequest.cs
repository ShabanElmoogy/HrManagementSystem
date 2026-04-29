using HrManagementSystem.Entities.BasicEntities;
using NewsProject.Server.Seeds;

namespace HrManagementSystem.Persistance.Seeds
{
    public static class SeedsRequest
    {
        public static async Task<WebApplication> AddSeedsRequest(this WebApplication webApplication)
        {
            var scopeFactory = webApplication.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await DefaultRoles.SeedRolesAsync(roleManager);
            await DefaultUsers.SeedViewerUserAsync(userManager);
            await DefaultUsers.SeedAdminUserAsync(userManager, roleManager);

            return webApplication;
        }
    }
}
