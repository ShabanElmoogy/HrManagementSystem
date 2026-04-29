using HrManagementSystem.Contracts.BasicContracts.KanbanCards;

namespace HrManagementSystem.Contracts.BasicContracts.KanbanColumns
{
    public record KanbanColumnResponse(
        int Id,
        int KanbanBoardId,
        string Name,
        int Order,
        bool IsArchived,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted,
        IEnumerable<SimpleKanbanCardResponse> Cards
    );

    public record SimpleKanbanColumnResponse(
        int Id,
        string Name,
        int Order,
        bool IsArchived
    );
}