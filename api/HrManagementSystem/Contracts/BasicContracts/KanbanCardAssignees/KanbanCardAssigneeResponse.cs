namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

public record KanbanCardAssigneeResponse(
    int Id,
    int KanbanCardId,
    string KanbanCardTitle,
    string UserId,
    string UserName,
    string UserEmail,
    DateTime CreatedOn,
    DateTime? UpdatedOn,
    bool IsDeleted
);
