using HrManagementSystem.Contracts.BasicContracts.KanbanLabels;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanLabelErrors(IStringLocalizer<KanbanLabelRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanLabelRequest> _localizer = localizer;

        public Error KanbanLabelNotFound =>
            new("KanbanLabel.NotFound", _localizer[nameof(KanbanLabelNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanLabelExists =>
            new("KanbanLabel.Duplicated", _localizer[nameof(KanbanLabelExists)], StatusCodes.Status409Conflict);

        public Error KanbanLabelInUse =>
            new("KanbanLabel.InUse", _localizer[nameof(KanbanLabelInUse)], StatusCodes.Status400BadRequest);
    }
}
