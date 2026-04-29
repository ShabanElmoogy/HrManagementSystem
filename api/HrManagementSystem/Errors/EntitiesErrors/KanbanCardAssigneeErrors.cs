using HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanCardAssigneeErrors(IStringLocalizer<KanbanCardAssigneeRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanCardAssigneeRequest> _localizer = localizer;

        public Error KanbanCardAssigneeNotFound =>
            new("KanbanCardAssignee.NotFound", _localizer[nameof(KanbanCardAssigneeNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanCardAssigneeExists =>
            new("KanbanCardAssignee.Duplicated", _localizer[nameof(KanbanCardAssigneeExists)], StatusCodes.Status409Conflict);

        public Error KanbanCardNotFound =>
            new("KanbanCardAssignee.CardNotFound", _localizer[nameof(KanbanCardNotFound)], StatusCodes.Status404NotFound);

        public Error UserNotFound =>
            new("KanbanCardAssignee.UserNotFound", _localizer[nameof(UserNotFound)], StatusCodes.Status404NotFound);
    }
}
