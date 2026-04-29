using TaskStatus = HrManagementSystem.Consts.TaskStatus;

namespace HrManagementSystem.Contracts.BasicContracts.BoardTasks
{
    public class BoardTaskRequestValidator : AbstractValidator<BoardTaskRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<BoardTaskRequest> _localizer;

        public BoardTaskRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<BoardTaskRequest> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(t => t.Title)
                .Trimmed()
                .NotEmpty()
                .Length(3, 200)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(t => t.Status)
                .InclusiveBetween((int)TaskStatus.Todo, (int)TaskStatus.Done)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(t => t.Priority)
                .InclusiveBetween((int)TaskPriority.Low, (int)TaskPriority.Critical)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(t => t.EstimatedHours)
                .GreaterThanOrEqualTo(0).When(t => t.EstimatedHours.HasValue)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(t => t.LoggedHours)
                .GreaterThanOrEqualTo(0).When(t => t.LoggedHours.HasValue)
                .WithMessage(_localizer[Strings.InvalidValues]);

            RuleFor(t => t)
                .Must(t => AreReferencesValid(t))
                .WithMessage(_localizer[Strings.InvalidValues]);
        }

        private bool AreReferencesValid(BoardTaskRequest t)
        {
            if (t.KanbanBoardId.HasValue)
            {
                if (!_dbContext.KanbanBoards.Any(b => b.Id == t.KanbanBoardId && !b.IsDeleted))
                    return false;
            }

            if (t.KanbanColumnId.HasValue)
            {
                if (!_dbContext.KanbanColumns.Any(c => c.Id == t.KanbanColumnId && !c.IsDeleted))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(t.AssigneeId))
            {
                if (!_dbContext.Users.Any(u => u.Id == t.AssigneeId))
                    return false;
            }

            return true;
        }
    }
}