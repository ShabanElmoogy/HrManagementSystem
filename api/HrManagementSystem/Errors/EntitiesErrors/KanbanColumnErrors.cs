using HrManagementSystem.Contracts.BasicContracts.KanbanColumns;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanColumnErrors(IStringLocalizer<KanbanColumnRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanColumnRequest> _localizer = localizer;

        public Error KanbanColumnNotFound =>
            new("KanbanColumn.NotFound", _localizer[nameof(KanbanColumnNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanColumnExists =>
            new("KanbanColumn.Duplicated", _localizer[nameof(KanbanColumnExists)], StatusCodes.Status409Conflict);

        public Error KanbanColumnHasCards =>
            new("KanbanColumn.HasCards", _localizer[nameof(KanbanColumnHasCards)], StatusCodes.Status400BadRequest);
    }
}
