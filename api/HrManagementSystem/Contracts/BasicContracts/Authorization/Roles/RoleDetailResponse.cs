namespace HrManagementSystem.Contracts.BasicContracts.Authorization.Roles
{
    public record RoleDetailResponse(
        string Id,
        string Name,
        bool IsDeleted,
        IEnumerable<string> Permissions
    );
}