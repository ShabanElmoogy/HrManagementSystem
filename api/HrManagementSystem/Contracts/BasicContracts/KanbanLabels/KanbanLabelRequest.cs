namespace HrManagementSystem.Contracts.BasicContracts.KanbanLabels
{
    public record KanbanLabelRequest(
        int Id,
        int KanbanBoardId,
        string Name,
        string ColorHex
    );
}