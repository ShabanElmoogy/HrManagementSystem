namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;

public record SimpleKanbanCardCommentResponse(
    int Id,
    string CommentText,
    string UserId,
    string UserName,
    DateTime CreatedOn
);
