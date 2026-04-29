namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;

public class KanbanCardCommentRequestValidator : AbstractValidator<KanbanCardCommentRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<KanbanCardCommentRequest> _localizer;

    public KanbanCardCommentRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanCardCommentRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.KanbanCardId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(CardExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x.CommentText)
            .Trimmed()
            .NotEmpty()
            .WithMessage(_localizer[Strings.Required])
            .Length(1, 2000)
            .WithMessage(_localizer[Strings.MaxLengthError]);
    }

    private bool CardExists(int cardId)
    {
        return _dbContext.KanbanCards.Any(c => c.Id == cardId && !c.IsDeleted);
    }
}
