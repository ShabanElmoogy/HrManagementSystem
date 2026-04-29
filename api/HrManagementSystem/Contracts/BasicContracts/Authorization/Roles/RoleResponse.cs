namespace HrManagementSystem.Contracts.BasicContracts.Authorization.Roles
{
    public record RoleResponse
    (
        string Id,
        string Name,
        bool IsDeleted,
        List<CheckBoxViewModel>? RoleClaims
    );


}
