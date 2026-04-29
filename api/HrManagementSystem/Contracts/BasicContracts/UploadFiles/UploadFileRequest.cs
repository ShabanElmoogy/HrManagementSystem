namespace HrManagementSystem.Contracts.BasicContracts.UploadFiles
{
    public record UploadFileRequest(
        IFormFile File
    );
}