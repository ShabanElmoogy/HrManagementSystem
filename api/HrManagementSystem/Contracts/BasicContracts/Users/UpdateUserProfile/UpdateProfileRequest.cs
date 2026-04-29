namespace HrManagementSystem.Contracts.BasicContracts.Users.UpdateUserProfile
{
    public record UpdateProfileRequest(
        string? Id,
        string UserName,
        string FirstName,
        string LastName
    );
}