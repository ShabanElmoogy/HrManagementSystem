namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskComments
{
    public record BoardTaskCommentRequest(
        int Id,
        int BoardTaskId,
        string CommentText
    );
}
