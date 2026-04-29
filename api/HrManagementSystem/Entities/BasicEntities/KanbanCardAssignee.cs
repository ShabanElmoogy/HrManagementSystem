namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// المستخدمين المعينين على كارت معين
/// </summary>
public class KanbanCardAssignee : AuditableEntity
{
    public int Id { get; set; }

    public int KanbanCardId { get; set; }
    public KanbanCard KanbanCard { get; set; } = default!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
}
