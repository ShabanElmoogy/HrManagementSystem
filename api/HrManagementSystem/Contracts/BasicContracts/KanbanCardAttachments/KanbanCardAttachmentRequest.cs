namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments
{
    public record KanbanCardAttachmentRequest(
        int Id,
        int KanbanCardId,
        Guid UploadedFileId
    );
}
