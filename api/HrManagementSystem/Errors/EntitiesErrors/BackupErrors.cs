using HrManagementSystem.Contracts.BasicContracts.ArchiveData;

namespace HrManagementSystem.Errors.EntitiesErrors
{
    public class BackupErrors(IStringLocalizer<BackupRequest> localizer)
    {
        private readonly IStringLocalizer<BackupRequest> _localizer = localizer;

        public Error BackupNotFound =>
                new("Backup.NotFound", _localizer[nameof(BackupNotFound)], StatusCodes.Status404NotFound);

        public Error BackupExists =>
                new("Backup.Duplicated", _localizer[nameof(BackupExists)], StatusCodes.Status409Conflict);

        public Error BackupError =>
                new("Backup.BackupError", _localizer[nameof(BackupError)], StatusCodes.Status500InternalServerError);

        public Error BackupFileNotFound =>
                new("Backup.FileNotFound", _localizer[nameof(BackupFileNotFound)], StatusCodes.Status404NotFound);

        public Error BackupRestoreError =>
                new("Backup.RestoreError", _localizer[nameof(BackupRestoreError)], StatusCodes.Status500InternalServerError);

        public Error DatabaseInUse =>
                new("Backup.DatabaseInUse", _localizer[nameof(DatabaseInUse)], StatusCodes.Status409Conflict);

        public Error NoBackupsFound =>
                new("Backup.NoBackupsFound", _localizer[nameof(NoBackupsFound)], StatusCodes.Status404NotFound);

        public Error InvalidIntervalValue =>
                new("Backup.InvalidIntervalValue", _localizer[nameof(InvalidIntervalValue)], StatusCodes.Status500InternalServerError);
    }
}
