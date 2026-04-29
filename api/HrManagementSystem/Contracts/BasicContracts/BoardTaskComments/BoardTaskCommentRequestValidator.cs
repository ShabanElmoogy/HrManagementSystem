namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskComments;

public class BoardTaskCommentRequestValidator : AbstractValidator<BoardTaskCommentRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<BoardTaskCommentRequest> _localizer;

    public BoardTaskCommentRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<BoardTaskCommentRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.BoardTaskId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(TaskExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x.CommentText)
            .Trimmed()
            .NotEmpty()
            .WithMessage(_localizer[Strings.Required])
            .Length(1, 2000)
            .WithMessage(_localizer[Strings.MaxLengthError]);
    }

    private bool TaskExists(int taskId)
    {
        return _dbContext.BoardTasks.Any(t => t.Id == taskId && !t.IsDeleted);
    }
}
