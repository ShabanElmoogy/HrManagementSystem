using HrManagementSystem.Contracts.BasicContracts.KanbanLabels;

namespace HrManagementSystem.Services.BasicServices.KanbanLabelsService;

public interface IKanbanLabelService
{
    Task<IEnumerable<KanbanLabelResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanLabelResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<KanbanLabelResponse>> AddAsync(KanbanLabelRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanLabelResponse>> UpdateAsync(KanbanLabelRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}