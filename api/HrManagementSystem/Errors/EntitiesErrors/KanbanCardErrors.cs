using HrManagementSystem.Contracts.BasicContracts.KanbanCards;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanCardErrors(IStringLocalizer<KanbanCardRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanCardRequest> _localizer = localizer;

        public Error KanbanCardNotFound =>
            new("KanbanCard.NotFound", _localizer[nameof(KanbanCardNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanCardExists =>
            new("KanbanCard.Duplicated", _localizer[nameof(KanbanCardExists)], StatusCodes.Status409Conflict);

        public Error KanbanCardHasAssignees =>
            new("KanbanCard.HasAssignees", _localizer[nameof(KanbanCardHasAssignees)], StatusCodes.Status400BadRequest);

        public Error InvalidColumn =>
            new("KanbanCard.InvalidColumn", _localizer[nameof(InvalidColumn)], StatusCodes.Status400BadRequest);
    }
}
