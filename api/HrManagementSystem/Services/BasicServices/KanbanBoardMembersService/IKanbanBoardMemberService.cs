using HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers;

namespace HrManagementSystem.Services.BasicServices.KanbanBoardMembersService;

public interface IKanbanBoardMemberService
{
    Task<IEnumerable<KanbanBoardMemberResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<KanbanBoardMemberResponse>> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<KanbanBoardMemberResponse>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken);
    Task<Result<KanbanBoardMemberResponse>> AddAsync(KanbanBoardMemberRequest request, CancellationToken cancellationToken = default);
    Task<Result<KanbanBoardMemberResponse>> UpdateAsync(KanbanBoardMemberRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
}
