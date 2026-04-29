namespace HrManagementSystem.Contracts.BasicContracts.KanbanCards
{
    public class KanbanCardRequestValidator : AbstractValidator<KanbanCardRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<KanbanCardRequest> _localizer;

        public KanbanCardRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanCardRequest> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(c => c.Title)
                .Trimmed()
                .NotEmpty()
                .Length(3, 200)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(c => c.Description)
                .Trimmed()
                .MaximumLength(1000)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(c => c.KanbanColumnId)
                .GreaterThan(0)
                .WithMessage(_localizer[Strings.GreaterThanZero])
                .Must(ColumnExists)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(c => c.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage(_localizer[Strings.GreaterThanZero]);
        }

        private bool ColumnExists(int columnId)
        {
            return _dbContext.KanbanColumns.Any(c => c.Id == columnId && !c.IsDeleted);
        }
    }
}
