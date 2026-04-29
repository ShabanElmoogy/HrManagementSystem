using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Persistance.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedViewerUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "user",
                Email = "user@user.com",
                FirstName = "user",
                LastName = "user",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, AppRoles.user);
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManger)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
                FirstName = "admin",
                LastName = "admin",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, AppRoles.admin);
            }

            await roleManger.SeedClaimsForSuperUser();
        }

        private static async Task SeedClaimsForSuperUser(this RoleManager<ApplicationRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(AppRoles.admin.ToString());
            await roleManager.AddPermissionClaims(adminRole);
        }

        public static async Task AddPermissionClaims(this RoleManager<ApplicationRole> roleManager, ApplicationRole role)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GetAllPermissions();

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == Permissions.Type && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Permissions.Type, permission));
            }
        }
    }
}
