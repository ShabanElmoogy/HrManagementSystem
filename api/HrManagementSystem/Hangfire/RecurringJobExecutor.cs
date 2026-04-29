using HrManagementSystem.Services.BasicServices.ArchiveDataService;

namespace HrManagementSystem.Hangfire
{
    public class RecurringJobExecutor
    {
        private readonly IBackupService _backupService;
        private readonly ILogger<RecurringJobExecutor> _logger;

        public RecurringJobExecutor(IBackupService backupService, ILogger<RecurringJobExecutor> logger)
        {
            _backupService = backupService;
            _logger = logger;
        }
        public async Task RunDatabaseBackup()
        {
            await _backupService.CreateBackupAsync("DatabaseBackup", cancellationToken: default);
        }
    }
}
