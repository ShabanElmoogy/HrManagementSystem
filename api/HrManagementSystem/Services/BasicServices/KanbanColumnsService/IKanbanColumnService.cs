using HrManagementSystem.Contracts.BasicContracts.KanbanColumns;

namespace HrManagementSystem.Services.BasicServices.KanbanColumnsService;

public interface IKanbanColumnService
{
    Task<IEnumerable<KanbanColumnResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanColumnResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<KanbanColumnResponse>> AddAsync(KanbanColumnRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanColumnResponse>> UpdateAsync(KanbanColumnRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}