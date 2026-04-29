namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardComments
{
    public record KanbanCardCommentResponse(
        int Id,
        int KanbanCardId,
        string KanbanCardTitle,
        string CommentText,
        string UserId,
        string UserName,
        string UserEmail,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );
}
