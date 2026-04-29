namespace HrManagementSystem.Contracts.BasicContracts.Users
{
    public record UserProfileResponse(
        string? id,
        string Email,
        string UserName,
        string FirstName,
        string LastName
    );
}