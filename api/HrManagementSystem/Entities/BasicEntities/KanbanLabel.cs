namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// التصنيفات داخل اللوحة
/// </summary>
public class KanbanLabel : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanBoardId { get; set; }
    public KanbanBoard KanbanBoard { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#808080";

    public ICollection<KanbanCardLabel> CardLabels { get; set; } = [];
}
