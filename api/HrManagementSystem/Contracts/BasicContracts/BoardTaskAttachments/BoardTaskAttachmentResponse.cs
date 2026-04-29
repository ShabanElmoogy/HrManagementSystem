namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments
{
    public record BoardTaskAttachmentResponse(
        int Id,
        int BoardTaskId,
        string BoardTaskTitle,
        Guid UploadedFileId,
        string FileName,
        string FileUrl,
        long FileSize,
        string ContentType,
        DateTime UploadedOn,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );
}
