namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

public record SimpleKanbanCardAssigneeResponse(
    int Id,
    string UserId,
    string UserName
);
