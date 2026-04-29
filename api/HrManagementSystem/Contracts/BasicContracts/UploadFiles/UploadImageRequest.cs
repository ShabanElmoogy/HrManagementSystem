namespace HrManagementSystem.Contracts.BasicContracts.UploadFiles
{
    public record UploadImageRequest(
        IFormFile Image
    );
}