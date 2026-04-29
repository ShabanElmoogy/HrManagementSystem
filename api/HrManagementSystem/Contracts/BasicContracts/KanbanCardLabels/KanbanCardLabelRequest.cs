namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels
{
    public record KanbanCardLabelRequest(
        int Id,
        int KanbanCardId,
        int KanbanLabelId
    );
}
