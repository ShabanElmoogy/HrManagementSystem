namespace HrManagementSystem.Contracts.BasicContracts.Users.CreateUser
{
    public record CreateUserRequest(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        IList<string> Roles
    );
}