namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers
{
    public record KanbanBoardMemberRequest(
        int Id,
        int KanbanBoardId,
        string UserId,
        KanbanBoardRole Role
    );
}
