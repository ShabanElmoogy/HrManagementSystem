namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// مرفقات الكارت
/// </summary>
public class KanbanCardAttachment : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanCardId { get; set; }
    public KanbanCard KanbanCard { get; set; } = default!;

    public Guid UploadedFileId { get; set; }
    public UploadedFile UploadedFile { get; set; } = default!;
}
