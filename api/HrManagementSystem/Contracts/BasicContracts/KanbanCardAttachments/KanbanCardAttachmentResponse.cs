namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments
{
    public record KanbanCardAttachmentResponse(
        int Id,
        int KanbanCardId,
        string KanbanCardTitle,
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
