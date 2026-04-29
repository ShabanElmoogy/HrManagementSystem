namespace HrManagementSystem.Authorization.Filters
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {
    }
}