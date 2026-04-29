using HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;

namespace HrManagementSystem.Services.BasicServices.KanbanCardAttachmentsService;

public interface IKanbanCardAttachmentService
{
    Task<IEnumerable<KanbanCardAttachmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<KanbanCardAttachmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<KanbanCardAttachmentResponse>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardAttachmentResponse>> AddAsync(KanbanCardAttachmentRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanCardAttachmentResponse>> UpdateAsync(KanbanCardAttachmentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
}
