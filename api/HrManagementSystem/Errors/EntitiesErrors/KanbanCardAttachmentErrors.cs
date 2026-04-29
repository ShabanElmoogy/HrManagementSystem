using HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;

namespace HrManagementSystem.Errors.EntitiesErrors;

public class KanbanCardAttachmentErrors(IStringLocalizer<KanbanCardAttachmentRequest> localizer)
{
    private readonly IStringLocalizer<KanbanCardAttachmentRequest> _localizer = localizer;

    public Error KanbanCardAttachmentNotFound =>
        new("KanbanCardAttachment.NotFound", _localizer[nameof(KanbanCardAttachmentNotFound)], StatusCodes.Status404NotFound);

    public Error KanbanCardAttachmentExists =>
        new("KanbanCardAttachment.Duplicated", _localizer[nameof(KanbanCardAttachmentExists)], StatusCodes.Status409Conflict);

    public Error InvalidKanbanCard =>
        new("KanbanCardAttachment.InvalidCard", _localizer[nameof(InvalidKanbanCard)], StatusCodes.Status400BadRequest);

    public Error InvalidFile =>
        new("KanbanCardAttachment.InvalidFile", _localizer[nameof(InvalidFile)], StatusCodes.Status400BadRequest);
}
