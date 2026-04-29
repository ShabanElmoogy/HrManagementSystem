using HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

namespace HrManagementSystem.Errors.EntitiesErrors;

public class KanbanCardLabelErrors(IStringLocalizer<KanbanCardLabelRequest> localizer)
{
    private readonly IStringLocalizer<KanbanCardLabelRequest> _localizer = localizer;

    public Error KanbanCardLabelNotFound =>
        new("KanbanCardLabel.NotFound", _localizer[nameof(KanbanCardLabelNotFound)], StatusCodes.Status404NotFound);

    public Error InvalidKanbanCard =>
        new("KanbanCardLabel.InvalidCard", _localizer[nameof(InvalidKanbanCard)], StatusCodes.Status400BadRequest);

    public Error InvalidKanbanLabel =>
        new("KanbanCardLabel.InvalidLabel", _localizer[nameof(InvalidKanbanLabel)], StatusCodes.Status400BadRequest);

    public Error KanbanCardLabelExists =>
        new("KanbanCardLabel.Exists", _localizer[nameof(KanbanCardLabelExists)], StatusCodes.Status409Conflict);
}
