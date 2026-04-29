namespace HrManagementSystem.Contracts.BasicContracts.UploadFiles
{
    public record UploadManyFilesRequest(
        IFormFileCollection Files
    );
}