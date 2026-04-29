namespace HrManagementSystem.Authorization.Filters
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; private set; } = permission;
    }
}