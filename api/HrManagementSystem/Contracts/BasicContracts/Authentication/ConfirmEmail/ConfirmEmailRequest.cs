namespace HrManagementSystem.Contracts.BasicContracts.Authentication.ConfirmEmail
{
    public record ConfirmEmailRequest(
        string UserId,
        string Code
    );
}