namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// كارت يمثل مهمة داخل عمود
/// </summary>
public class KanbanCard : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanColumnId { get; set; }
    public KanbanColumn KanbanColumn { get; set; } = default!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<KanbanCardAssignee> Assignees { get; set; } = [];
    public ICollection<KanbanCardLabel> CardLabels { get; set; } = [];
    public ICollection<KanbanCardComment> Comments { get; set; } = [];
    public ICollection<KanbanCardAttachment> Attachments { get; set; } = [];
}
