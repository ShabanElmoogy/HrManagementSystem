namespace HrManagementSystem.Contracts.BasicContracts.Authentication.ResetPassword
{
    public record ResetPasswordRequest(
        string Email,
        string Code,
        string NewPassword
    );
}