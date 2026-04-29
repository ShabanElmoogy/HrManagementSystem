namespace HrManagementSystem.Contracts.BasicContracts.Authorization.Roles
{
    public record RoleRequest(
        string? Id,
        string Name,
        List<CheckBoxViewModel>? RoleClaims
    );
}
