namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoards
{
    public record KanbanBoardRequest(
        int Id,
        string Name,
        string Description
    );
}