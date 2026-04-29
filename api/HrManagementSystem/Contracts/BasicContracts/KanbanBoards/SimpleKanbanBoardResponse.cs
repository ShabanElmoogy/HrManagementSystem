namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoards
{
    public record SimpleKanbanBoardResponse(
        int Id,
        string NameAr,
        string NameEn,
        bool IsDeleted
    );
}