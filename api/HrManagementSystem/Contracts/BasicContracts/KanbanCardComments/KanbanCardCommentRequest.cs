namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardComments
{
    public record KanbanCardCommentRequest(
        int Id,
        int KanbanCardId,
        string CommentText
    );
}
