using HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;

namespace HrManagementSystem.Services.BasicServices.KanbanCardCommentsService;

public interface IKanbanCardCommentService
{
    Task<IEnumerable<KanbanCardCommentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<KanbanCardCommentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<KanbanCardCommentResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardCommentResponse>> AddAsync(KanbanCardCommentRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardCommentResponse>> UpdateAsync(KanbanCardCommentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
}
