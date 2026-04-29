namespace HrManagementSystem.Contracts.BasicContracts.Authentication.Register
{
    public record RegisterRequest
        (
         string FirstName,
         string LastName,
         string UserName,
         string Email,
         string Password,
         byte[]? ProfilePicture
        );
}
