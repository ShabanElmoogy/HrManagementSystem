using HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;

namespace HrManagementSystem.Services.BasicServices.BoardTaskAttachmentsService;

public interface IBoardTaskAttachmentService
{
    Task<IEnumerable<BoardTaskAttachmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<BoardTaskAttachmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardTaskAttachmentResponse>> GetByTaskIdAsync(int taskId, CancellationToken cancellationToken = default);
    Task<Result<BoardTaskAttachmentResponse>> AddAsync(BoardTaskAttachmentRequest request, CancellationToken cancellationToken = default);
    Task<Result<BoardTaskAttachmentResponse>> UpdateAsync(BoardTaskAttachmentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
}
