namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskComments
{
    public record BoardTaskCommentResponse(
        int Id,
        int BoardTaskId,
        string BoardTaskTitle,
        string CommentText,
        string UserId,
        string UserName,
        string UserEmail,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );
}
