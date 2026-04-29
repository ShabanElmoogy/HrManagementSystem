using HrManagementSystem.Contracts.BasicContracts.KanbanBoards;

namespace HrManagementSystem.Services.BasicServices.KanbanBoardsService;

public interface IKanbanBoardService
{
    Task<IEnumerable<KanbanBoardResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanBoardResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<KanbanBoardResponse>> AddAsync(KanbanBoardRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanBoardResponse>> UpdateAsync(KanbanBoardRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}