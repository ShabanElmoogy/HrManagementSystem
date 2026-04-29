namespace HrManagementSystem.Contracts.BasicContracts.BoardTasks
{
    public record BoardTaskResponse(
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
        int? KanbanColumnId,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted
    );

    public record SimpleBoardTaskResponse(
        int Id,
        string Title,
        int Status,
        int Priority,
        DateTime? DueDate,
        int Position
    );
}