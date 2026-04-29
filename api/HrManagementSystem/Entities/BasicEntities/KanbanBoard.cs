namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// لوحة كانبان تحتوي على أعمدة ومهام
/// </summary>
public class KanbanBoard : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? BackgroundColor { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<KanbanColumn> Columns { get; set; } = [];
    public ICollection<KanbanBoardMember> Members { get; set; } = [];
    public ICollection<KanbanLabel> Labels { get; set; } = [];
    public ICollection<BoardTask> Tasks { get; set; } = [];
}
