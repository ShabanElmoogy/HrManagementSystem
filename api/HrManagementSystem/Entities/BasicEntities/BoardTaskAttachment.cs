namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// مرفقات المهام (صور / ملفات)
/// </summary>
public class BoardTaskAttachment : AuditableEntity
{
    public int Id { get; set; }

    public int BoardTaskId { get; set; }
    public BoardTask BoardTask { get; set; } = default!;

    public Guid UploadedFileId { get; set; }
    public UploadedFile UploadedFile { get; set; } = default!;
}
