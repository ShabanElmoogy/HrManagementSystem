using HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;

namespace HrManagementSystem.Errors.EntitiesErrors;

public class KanbanCardCommentErrors(IStringLocalizer<KanbanCardCommentRequest> localizer)
{
    private readonly IStringLocalizer<KanbanCardCommentRequest> _localizer = localizer;

    public Error KanbanCardCommentNotFound =>
        new("KanbanCardComment.NotFound", _localizer[nameof(KanbanCardCommentNotFound)], StatusCodes.Status404NotFound);

    public Error InvalidKanbanCard =>
        new("KanbanCardComment.InvalidCard", _localizer[nameof(InvalidKanbanCard)], StatusCodes.Status400BadRequest);

    public Error UnauthorizedCommentAccess =>
        new("KanbanCardComment.Unauthorized", _localizer[nameof(UnauthorizedCommentAccess)], StatusCodes.Status403Forbidden);
}
