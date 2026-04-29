namespace HrManagementSystem.Contracts.BasicContracts.Users.UpdateUser
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        IList<string> Roles
    );
}