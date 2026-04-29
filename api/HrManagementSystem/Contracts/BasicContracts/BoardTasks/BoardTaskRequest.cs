namespace HrManagementSystem.Contracts.BasicContracts.BoardTasks
{
    public record BoardTaskRequest(
        int Id,
        string Title,
        string Description,
        int Status,
        int Priority,
        DateTime? DueDate,
        decimal? EstimatedHours,
        decimal? LoggedHours,
        string? AssigneeId,
        int Position,
        int? KanbanBoardId,
        int? KanbanColumnId
    );
}