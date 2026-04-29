using TaskStatus = HrManagementSystem.Consts.TaskStatus;

namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// مهمة يمكن أن تكون داخل بورد أو مستقلة، مثل "بوست"
/// </summary>
public class BoardTask : AuditableEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? LoggedHours { get; set; }

    /// <summary>
    /// الشخص المكلف بالمهمة
    /// </summary>
    public string? AssigneeId { get; set; }
    public ApplicationUser? Assignee { get; set; }

    /// <summary>
    /// ترتيب المهمة داخل العمود
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// العلاقة مع البورد (اختياري)
    /// </summary>
    public int? KanbanBoardId { get; set; }
    public KanbanBoard? KanbanBoard { get; set; }

    /// <summary>
    /// العلاقة مع العمود (اختياري)
    /// </summary>
    public int? KanbanColumnId { get; set; }
    public KanbanColumn? KanbanColumn { get; set; }

    /// <summary>
    /// التعليقات على المهمة (زى بوست فيسبوك)
    /// </summary>
    public ICollection<BoardTaskComment> Comments { get; set; } = [];

    /// <summary>
    /// المرفقات الخاصة بالمهمة
    /// </summary>
    public ICollection<BoardTaskAttachment> Attachments { get; set; } = [];
}
