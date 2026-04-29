namespace HrManagementSystem.Contracts.BasicContracts.Users.ChangePassword
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword
    );
}