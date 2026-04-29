namespace HrManagementSystem.Contracts.BasicContracts.KanbanLabels
{
    public class KanbanLabelRequestValidator : AbstractValidator<KanbanLabelRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<KanbanLabelRequest> _localizer;

        public KanbanLabelRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanLabelRequest> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(l => l.KanbanBoardId)
                .GreaterThan(0)
                .WithMessage(_localizer[Strings.GreaterThanZero])
                .Must(BoardExists)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(l => l.Name)
                .Trimmed()
                .NotEmpty()
                .Length(2, 100)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(l => l.ColorHex)
                .NotEmpty()
                .Matches("^#[0-9A-Fa-f]{6}$")
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(l => l)
                .Must(l => !IsDuplicateNameInBoard(l))
                .WithMessage(_localizer[Strings.DuplicatedValue]);
        }

        private bool BoardExists(int boardId)
        {
            return _dbContext.KanbanBoards.Any(b => b.Id == boardId && !b.IsDeleted);
        }

        private bool IsDuplicateNameInBoard(KanbanLabelRequest request)
        {
            return _dbContext.KanbanLabels.Any(l => l.KanbanBoardId == request.KanbanBoardId && l.Name == request.Name && l.Id != request.Id && !l.IsDeleted);
        }
    }
}