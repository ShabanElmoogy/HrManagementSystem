namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

public record KanbanCardAssigneeRequest(
    int Id,
    int KanbanCardId,
    string UserId
);
