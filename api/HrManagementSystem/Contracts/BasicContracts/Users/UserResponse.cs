namespace HrManagementSystem.Contracts.BasicContracts.Users
{
    public record UserResponse(
        string Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        bool IsDisabled,
        bool IsLocked,
        byte[]? ProfilePicture,
        IEnumerable<string> Roles
    );
}