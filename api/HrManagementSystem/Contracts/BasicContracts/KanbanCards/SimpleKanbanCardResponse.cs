namespace HrManagementSystem.Contracts.BasicContracts.KanbanCards
{
    public record SimpleKanbanCardResponse(
        int Id,
        string Title,
        string? Description,
        int Order,
        DateTime? DueDate,
        bool IsArchived
    );
}
