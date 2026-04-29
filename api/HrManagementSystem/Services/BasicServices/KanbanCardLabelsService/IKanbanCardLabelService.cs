using HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

namespace HrManagementSystem.Services.BasicServices.KanbanCardLabelsService;

public interface IKanbanCardLabelService
{
    Task<IEnumerable<KanbanCardLabelResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<KanbanCardLabelResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<KanbanCardLabelResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardLabelResponse>> AddAsync(KanbanCardLabelRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardLabelResponse>> UpdateAsync(KanbanCardLabelRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
}
