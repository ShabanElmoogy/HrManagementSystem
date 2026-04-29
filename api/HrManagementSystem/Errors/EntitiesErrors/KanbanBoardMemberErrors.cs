using HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class KanbanBoardMemberErrors(IStringLocalizer<KanbanBoardMemberRequest> localizer)
    {
        private readonly IStringLocalizer<KanbanBoardMemberRequest> _localizer = localizer;

        public Error KanbanBoardMemberNotFound =>
            new("KanbanBoardMember.NotFound", _localizer[nameof(KanbanBoardMemberNotFound)], StatusCodes.Status404NotFound);

        public Error KanbanBoardMemberExists =>
            new("KanbanBoardMember.Duplicated", _localizer[nameof(KanbanBoardMemberExists)], StatusCodes.Status409Conflict);

        public Error KanbanBoardNotFound =>
            new("KanbanBoardMember.BoardNotFound", _localizer[nameof(KanbanBoardNotFound)], StatusCodes.Status404NotFound);

        public Error UserNotFound =>
            new("KanbanBoardMember.UserNotFound", _localizer[nameof(UserNotFound)], StatusCodes.Status404NotFound);

        public Error InvalidRole =>
            new("KanbanBoardMember.InvalidRole", _localizer[nameof(InvalidRole)], StatusCodes.Status400BadRequest);
    }
}
