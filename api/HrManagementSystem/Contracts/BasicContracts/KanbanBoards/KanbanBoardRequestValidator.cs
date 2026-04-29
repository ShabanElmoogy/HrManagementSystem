namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoards
{
    public class KanbanBoardRequestValidator : AbstractValidator<KanbanBoardRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<KanbanBoardRequest> _localizer;

        public KanbanBoardRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanBoardRequest> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(c => c.Name)
                .Trimmed()
                .NotEmpty()
                .Length(3, 100)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(c => c)
             .Must(c => !IsNameDuplicated(c))
             .WithMessage(_localizer[Strings.DuplicatedValue]);

            RuleFor(c => c.Description)
                .Trimmed()
                .NotEmpty()
                .Length(3, 500)
                .WithMessage(_localizer[Strings.MaxLengthError]);

        }

        private bool IsNameDuplicated(KanbanBoardRequest board)
        {
            return _dbContext.KanbanBoards.Any(c => c.Name == board.Name && c.Id != board.Id);
        }
    }
}