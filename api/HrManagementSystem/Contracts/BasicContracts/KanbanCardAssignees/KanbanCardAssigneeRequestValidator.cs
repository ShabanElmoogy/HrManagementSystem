namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;

public class KanbanCardAssigneeRequestValidator : AbstractValidator<KanbanCardAssigneeRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<KanbanCardAssigneeRequest> _localizer;

    public KanbanCardAssigneeRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanCardAssigneeRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.KanbanCardId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(CardExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(_localizer[Strings.Required])
            .Must(UserExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x)
            .Must(x => !IsAssigneeDuplicated(x))
            .WithMessage(_localizer[Strings.DuplicatedValue]);
    }

    private bool CardExists(int cardId)
    {
        return _dbContext.KanbanCards.Any(c => c.Id == cardId && !c.IsDeleted);
    }

    private bool UserExists(string userId)
    {
        return _dbContext.Users.Any(u => u.Id == userId && !u.IsDisabled);
    }

    private bool IsAssigneeDuplicated(KanbanCardAssigneeRequest request)
    {
        return _dbContext.KanbanCardAssignees.Any(a => 
            a.KanbanCardId == request.KanbanCardId && 
            a.UserId == request.UserId && 
            a.Id != request.Id);
    }
}
