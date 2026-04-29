namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments
{
    public record BoardTaskAttachmentRequest(
        int Id,
        int BoardTaskId,
        Guid UploadedFileId
    );
}
