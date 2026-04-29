using HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

namespace HrManagementSystem.Contracts.BasicContracts.KanbanCards
{
    public record KanbanCardResponse(
        int Id,
        int KanbanColumnId,
        string Title,
        string? Description,
        int Order,
        DateTime? DueDate,
        bool IsArchived,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted,
        IEnumerable<KanbanCardAssigneeResponse> Assignees,
        IEnumerable<SimpleKanbanCardLabelResponse> CardLabels,
        IEnumerable<KanbanCardCommentResponse> Comments,
        IEnumerable<KanbanCardAttachmentResponse> Attachments
    );
}
