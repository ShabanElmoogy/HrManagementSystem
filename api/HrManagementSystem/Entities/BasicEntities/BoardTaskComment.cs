namespace HrManagementSystem.Entities.BasicEntities;

/// <summary>
/// تعليق على مهمة (زى الكومنت على بوست)
/// </summary>
public class BoardTaskComment : AuditableEntity
{
    public int Id { get; set; }

    public int BoardTaskId { get; set; }
    public BoardTask BoardTask { get; set; } = default!;

    public string CommentText { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;

    /// <summary>
    /// للردود داخل الكومنتات
    /// </summary>
    public int? ParentCommentId { get; set; }
    public BoardTaskComment? ParentComment { get; set; }
    public ICollection<BoardTaskComment> Replies { get; set; } = [];
}
