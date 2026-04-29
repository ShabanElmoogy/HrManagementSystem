namespace HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;

public class KanbanCardAttachmentRequestValidator : AbstractValidator<KanbanCardAttachmentRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<KanbanCardAttachmentRequest> _localizer;

    public KanbanCardAttachmentRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<KanbanCardAttachmentRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(x => x.KanbanCardId)
            .GreaterThan(0)
            .WithMessage(_localizer[Strings.GreaterThanZero])
            .Must(CardExists)
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

    private bool CardExists(int cardId)
    {
        return _dbContext.KanbanCards.Any(c => c.Id == cardId && !c.IsDeleted);
    }

    private bool FileExists(Guid fileId)
    {
        return _dbContext.Files.Any(f => f.Id == fileId && !f.IsDeleted);
    }

    private bool IsDuplicated(KanbanCardAttachmentRequest request)
    {
        return _dbContext.KanbanCardAttachments.Any(a => 
            a.KanbanCardId == request.KanbanCardId && 
            a.UploadedFileId == request.UploadedFileId && 
            a.Id != request.Id &&
            !a.IsDeleted);
    }
}
