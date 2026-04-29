namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments
{
    public record SimpleKanbanCardAttachmentResponse(
        int Id,
        string FileName,
        string FileUrl,
        DateTime UploadedOn
    );
}
