namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels
{
    public record SimpleKanbanCardLabelResponse(
        int Id,
        int KanbanCardId,
        int KanbanLabelId,
        string LabelName,
        string ColorHex
    );
}
