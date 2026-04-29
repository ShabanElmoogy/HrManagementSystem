using HrManagementSystem.Contracts.BasicContracts.KanbanCards;

namespace HrManagementSystem.Services.BasicServices.KanbanCardsService;

public interface IKanbanCardService
{
    Task<IEnumerable<KanbanCardResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanCardResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<KanbanCardResponse>> AddAsync(KanbanCardRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardResponse>> UpdateAsync(KanbanCardRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}
