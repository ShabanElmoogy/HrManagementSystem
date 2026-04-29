namespace HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;

public class BoardTaskAttachmentRequestValidator : AbstractValidator<BoardTaskAttachmentRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<BoardTaskAttachmentRequest> _localizer;

    public BoardTaskAttachmentRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<BoardTaskAttachmentRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.BoardTaskId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(TaskExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x.UploadedFileId)
            .NotEmpty()
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(FileExists)
            .WithMessage(_localizer[Strings.InvalidValues]);

        RuleFor(x => x)
            .Must(x => !IsDuplicated(x))
            .WithMessage(_localizer[Strings.DuplicatedValue]);
    }

    private bool TaskExists(int taskId)
    {
        return _dbContext.BoardTasks.Any(t => t.Id == taskId && !t.IsDeleted);
    }

    private bool FileExists(Guid fileId)
    {
        return _dbContext.Files.Any(f => f.Id == fileId && !f.IsDeleted);
    }

    private bool IsDuplicated(BoardTaskAttachmentRequest request)
    {
        return _dbContext.BoardTaskAttachments.Any(a =>
            a.BoardTaskId == request.BoardTaskId &&
            a.UploadedFileId == request.UploadedFileId &&
            a.Id != request.Id &&
            !a.IsDeleted);
    }
}
