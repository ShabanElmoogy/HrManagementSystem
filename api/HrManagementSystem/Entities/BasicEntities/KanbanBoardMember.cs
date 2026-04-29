namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// عضوية المستخدم داخل لوحة معينة
/// </summary>
public class KanbanBoardMember : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanBoardId { get; set; }
    public KanbanBoard KanbanBoard { get; set; } = default!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;

    public KanbanBoardRole Role { get; set; } = KanbanBoardRole.Viewer;
}
