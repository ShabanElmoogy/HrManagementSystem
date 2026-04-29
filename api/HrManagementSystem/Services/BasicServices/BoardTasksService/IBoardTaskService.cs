using HrManagementSystem.Contracts.BasicContracts.BoardTasks;

namespace HrManagementSystem.Services.BasicServices.BoardTasksService;

public interface IBoardTaskService
{
    Task<IEnumerable<BoardTaskResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<BoardTaskResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<BoardTaskResponse>> AddAsync(BoardTaskRequest request, CancellationToken cancellationToken = default);
    Task<Result<BoardTaskResponse>> UpdateAsync(BoardTaskRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}