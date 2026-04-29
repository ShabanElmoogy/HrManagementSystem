using HrManagementSystem.Contracts.BasicContracts.BoardTaskComments;

namespace HrManagementSystem.Services.BasicServices.BoardTaskCommentsService;

public interface IBoardTaskCommentService
{
    Task<IEnumerable<BoardTaskCommentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<BoardTaskCommentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardTaskCommentResponse>> GetByTaskIdAsync(int taskId, CancellationToken cancellationToken = default);
    Task<Result<BoardTaskCommentResponse>> AddAsync(BoardTaskCommentRequest request, CancellationToken cancellationToken = default);
    Task<Result<BoardTaskCommentResponse>> UpdateAsync(BoardTaskCommentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
}
