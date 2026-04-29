namespace HrManagementSystem.Contracts.BasicContracts.UploadFiles
{
    public record UploadFileResponse
    (
        string Id,
        string FileName,
        string StoredFileName,
        string ContentType,
        string FileExtension,
        DateTime CreatedOn,
        string CreatedByPc,
        string CreatedById,
        bool IsDeleted
    );
}
