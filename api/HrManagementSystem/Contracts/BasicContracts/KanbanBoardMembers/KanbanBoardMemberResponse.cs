namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers
{
    public record KanbanBoardMemberResponse(
        int Id,
        int KanbanBoardId,
        string KanbanBoardName,
        string UserId,
        string UserName,
        string UserEmail,
        int Role,
        string RoleName,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );

    public record SimpleKanbanBoardMemberResponse(
        int Id,
        string UserId,
        string UserName,
        int Role,
        string RoleName
    );
}
