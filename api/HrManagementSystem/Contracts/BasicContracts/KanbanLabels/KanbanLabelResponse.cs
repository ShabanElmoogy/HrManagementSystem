namespace HrManagementSystem.Contracts.BasicContracts.KanbanLabels
{
    public record KanbanLabelResponse(
        int Id,
        int KanbanBoardId,
        string Name,
        string ColorHex,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );

    public record SimpleKanbanLabelResponse(
        int Id,
        string Name,
        string ColorHex
    );
}