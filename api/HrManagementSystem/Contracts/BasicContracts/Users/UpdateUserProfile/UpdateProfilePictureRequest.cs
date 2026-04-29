namespace HrManagementSystem.Contracts.BasicContracts.Users.UpdateUserProfile
{
    public record UpdateProfilePictureRequest(string? Id, byte[]? ProfilePicture);
}
