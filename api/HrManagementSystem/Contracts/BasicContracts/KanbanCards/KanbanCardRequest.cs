namespace HrManagementSystem.Contracts.BasicContracts.KanbanCards
{
    public record KanbanCardRequest(
        int Id,
        int KanbanColumnId,
        string Title,
        string? Description,
        int Order,
        DateTime? DueDate,
        bool IsArchived
    );
}
