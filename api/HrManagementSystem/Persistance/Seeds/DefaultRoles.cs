using HrManagementSystem.Entities.BasicEntities;

namespace NewsProject.Server.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new ApplicationRole(AppRoles.admin));
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = AppRoles.user,
                    IsDefault = true
                });
            }
        }
    }
}
