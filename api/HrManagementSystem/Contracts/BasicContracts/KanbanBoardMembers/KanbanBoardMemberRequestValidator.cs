namespace HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers;

public class KanbanBoardMemberRequestValidator : AbstractValidator<KanbanBoardMemberRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<KanbanBoardMemberRequest> _localizer;

    public KanbanBoardMemberRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanBoardMemberRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(m => m.KanbanBoardId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(BoardExists)
            .WithMessage(_localizer[Strings.Required]);

        RuleFor(m => m.UserId)
            .NotEmpty()
            .WithMessage(_localizer[Strings.Required])
            .Must(UserExists)
            .WithMessage(_localizer[Strings.UserNotFound]);

        RuleFor(m => m.Role)
            .IsInEnum()
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(m => m)
            .Must(m => !IsMembershipDuplicated(m))
            .WithMessage(_localizer[Strings.DuplicatedValue]);
    }

    private bool BoardExists(int boardId)
    {
        return _dbContext.KanbanBoards.Any(b => b.Id == boardId && !b.IsDeleted);
    }

    private bool UserExists(string userId)
    {
        return _dbContext.Users.Any(u => u.Id == userId && !u.IsDisabled);
    }

    private bool IsMembershipDuplicated(KanbanBoardMemberRequest member)
    {
        return _dbContext.KanbanBoardMembers.Any(m => 
            m.KanbanBoardId == member.KanbanBoardId && 
            m.UserId == member.UserId && 
            m.Id != member.Id &&
            !m.IsDeleted);
    }
}
