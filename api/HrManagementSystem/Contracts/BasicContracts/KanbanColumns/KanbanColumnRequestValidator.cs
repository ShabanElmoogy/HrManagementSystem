namespace HrManagementSystem.Contracts.BasicContracts.KanbanColumns
{
    public class KanbanColumnRequestValidator : AbstractValidator<KanbanColumnRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<KanbanColumnRequest> _localizer;

        public KanbanColumnRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanColumnRequest> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(c => c.KanbanBoardId)
                .GreaterThan(0)
                .WithMessage(_localizer[Strings.GreaterThanZero])
                .Must(BoardExists)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(c => c.Name)
                .Trimmed()
                .NotEmpty()
                .Length(3, 100)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(c => c.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage(_localizer[Strings.GreaterThanZero]);

            RuleFor(c => c)
                .Must(c => !IsDuplicateNameInBoard(c))
                .WithMessage(_localizer[Strings.DuplicatedValue]);
        }

        private bool BoardExists(int boardId)
        {
            return _dbContext.KanbanBoards.Any(b => b.Id == boardId && !b.IsDeleted);
        }

        private bool IsDuplicateNameInBoard(KanbanColumnRequest request)
        {
            return _dbContext.KanbanColumns.Any(col => col.KanbanBoardId == request.KanbanBoardId && col.Name == request.Name && col.Id != request.Id && !col.IsDeleted);
        }
    }
}