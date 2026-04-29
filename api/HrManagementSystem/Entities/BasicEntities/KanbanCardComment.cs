namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// التعليقات على الكارت
/// </summary>
public class KanbanCardComment : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanCardId { get; set; }
    public KanbanCard KanbanCard { get; set; } = default!;

    public string CommentText { get; set; } = string.Empty;
}
