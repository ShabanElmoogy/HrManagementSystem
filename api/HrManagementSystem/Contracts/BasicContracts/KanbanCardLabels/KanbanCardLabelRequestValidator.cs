namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardLabels;

public class KanbanCardLabelRequestValidator : AbstractValidator<KanbanCardLabelRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<KanbanCardLabelRequest> _localizer;

    public KanbanCardLabelRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanCardLabelRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.KanbanCardId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(CardExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x.KanbanLabelId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(LabelExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x)
            .Must(NotDuplicated)
            .WithMessage(_localizer[Strings.DuplicatedValue]);
    }

    private bool CardExists(int cardId)
    {
        return _dbContext.KanbanCards.Any(c => c.Id == cardId && !c.IsDeleted);
    }

    private bool LabelExists(int labelId)
    {
        return _dbContext.KanbanLabels.Any(l => l.Id == labelId && !l.IsDeleted);
    }

    private bool NotDuplicated(KanbanCardLabelRequest req)
    {
        return !_dbContext.KanbanCardLabels.Any(cl => cl.KanbanCardId == req.KanbanCardId && cl.KanbanLabelId == req.KanbanLabelId && cl.Id != req.Id && !cl.IsDeleted);
    }
}
