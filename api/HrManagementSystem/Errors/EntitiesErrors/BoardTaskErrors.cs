using HrManagementSystem.Contracts.BasicContracts.BoardTasks;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class BoardTaskErrors(IStringLocalizer<BoardTaskRequest> localizer)
    {
        private readonly IStringLocalizer<BoardTaskRequest> _localizer = localizer;

        public Error BoardTaskNotFound =>
            new("BoardTask.NotFound", _localizer[nameof(BoardTaskNotFound)], StatusCodes.Status404NotFound);

        public Error BoardTaskHasComments =>
            new("BoardTask.HasComments", _localizer[nameof(BoardTaskHasComments)], StatusCodes.Status400BadRequest);

        public Error BoardTaskHasAttachments =>
            new("BoardTask.HasAttachments", _localizer[nameof(BoardTaskHasAttachments)], StatusCodes.Status400BadRequest);
    }
}
