using HrManagementSystem.Contracts.BasicContracts.KanbanCards;

namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoards
{
    public record KanbanBoardResponse(
        int Id,
        string Name,
        string? Description,
        bool IsArchived,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted,
        IEnumerable<KanbanColumnResponse> Columns,
        IEnumerable<KanbanLabelResponse> Labels,
        IEnumerable<KanbanBoardMemberResponse> Members,
        IEnumerable<BoardTaskResponse> Tasks
    );

    public record KanbanColumnResponse(
        int Id,
        string Name,
        int Order,
        bool IsArchived,
        IEnumerable<SimpleKanbanCardResponse> Cards
    );

    public record KanbanLabelResponse(
        int Id,
        string Name,
        string ColorHex
    );

    public record KanbanBoardMemberResponse(
        int Id,
        string UserId,
        int Role
    );

    public record BoardTaskResponse(
        int Id,
        string Title,
        string Description,
        int Status,
        int Priority,
        DateTime? DueDate,
        string? AssigneeId,
        int Position
    );
}