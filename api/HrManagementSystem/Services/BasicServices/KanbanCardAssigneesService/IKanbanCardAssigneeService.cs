using HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

namespace HrManagementSystem.Services.BasicServices.KanbanCardAssigneesService;

public interface IKanbanCardAssigneeService
{
    Task<IEnumerable<KanbanCardAssigneeResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanCardAssigneeResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<KanbanCardAssigneeResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken);
    Task<Result<KanbanCardAssigneeResponse>> AddAsync(KanbanCardAssigneeRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardAssigneeResponse>> UpdateAsync(KanbanCardAssigneeRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}
