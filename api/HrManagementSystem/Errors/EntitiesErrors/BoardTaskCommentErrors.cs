using HrManagementSystem.Contracts.BasicContracts.BoardTaskComments;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class BoardTaskCommentErrors(IStringLocalizer<BoardTaskCommentRequest> localizer)
    {
        private readonly IStringLocalizer<BoardTaskCommentRequest> _localizer = localizer;

        public Error BoardTaskCommentNotFound =>
            new("BoardTaskComment.NotFound", _localizer[nameof(BoardTaskCommentNotFound)], StatusCodes.Status404NotFound);

        public Error InvalidBoardTask =>
            new("BoardTaskComment.InvalidTask", _localizer[nameof(InvalidBoardTask)], StatusCodes.Status400BadRequest);

        public Error UnauthorizedCommentAccess =>
            new("BoardTaskComment.Unauthorized", _localizer[nameof(UnauthorizedCommentAccess)], StatusCodes.Status403Forbidden);
    }
}
