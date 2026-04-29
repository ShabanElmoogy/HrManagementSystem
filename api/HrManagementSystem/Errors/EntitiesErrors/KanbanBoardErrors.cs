using HrManagementSystem.Contracts.BasicContracts.KanbanBoards;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanBoardErrors(IStringLocalizer<KanbanBoardRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanBoardRequest> _localizer = localizer;

        public Error KanbanBoardNotFound =>
            new("KanbanBoard.NotFound", _localizer[nameof(KanbanBoardNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanBoardExists =>
            new("KanbanBoard.Duplicated", _localizer[nameof(KanbanBoardExists)], StatusCodes.Status409Conflict);

        public Error KanbanBoardHasColumns =>
            new("KanbanBoard.HasColumns", _localizer[nameof(KanbanBoardHasColumns)], StatusCodes.Status400BadRequest);
    }
}
