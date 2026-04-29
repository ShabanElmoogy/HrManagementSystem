namespace HrManagementSystem.Contracts.BasicContracts.KanbanColumns
{
    public record KanbanColumnRequest(
        int Id,
        int KanbanBoardId,
        string Name,
        int Order,
        bool IsArchived
    );
}