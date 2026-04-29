namespace HrManagementSystem.Contracts.BasicContracts.ArchiveData
{
    public class BackupRequestValidator : AbstractValidator<BackupRequest>
    {
        private readonly IStringLocalizer<BackupRequest> _localizer;

        public BackupRequestValidator(IStringLocalizer<BackupRequest> localizer)
        {
            _localizer = localizer;

            RuleFor(b => b.FileName)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required])
                .MaximumLength(255)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(b => b.DatabaseName)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required])
                .MaximumLength(100)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(b => b.BackupPath)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required])
                .MaximumLength(255)
                .WithMessage(_localizer[Strings.MaxLengthError]);

            RuleFor(b => b.Status)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required])
                .MaximumLength(50)
                .WithMessage(_localizer[Strings.MaxLengthError])
                .Must(BeValidStatus)
                .WithMessage(_localizer[Strings.InvalidBackupStatus]);
        }

        private bool BeValidStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return false;

            string[] validStatuses = { "Completed", "Failed", "Deleted", "FileMissing", "Restored" };
            return validStatuses.Contains(status);
        }
    }
}