namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels
{
    public record KanbanCardLabelResponse(
        int Id,
        int KanbanCardId,
        int KanbanLabelId,
        string LabelName,
        string ColorHex,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );
}
