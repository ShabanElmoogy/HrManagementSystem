namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// الربط بين الكروت والتصنيفات
/// </summary>
public class KanbanCardLabel : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanCardId { get; set; }
    public KanbanCard KanbanCard { get; set; } = default!;

    public int KanbanLabelId { get; set; }
    public KanbanLabel KanbanLabel { get; set; } = default!;
}
