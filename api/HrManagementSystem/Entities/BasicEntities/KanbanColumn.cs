namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// عمود داخل لوحة كانبان
/// </summary>
public class KanbanColumn : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanBoardId { get; set; }
    public KanbanBoard KanbanBoard { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<KanbanCard> Cards { get; set; } = [];
    public ICollection<BoardTask> Tasks { get; set; } = [];
}
