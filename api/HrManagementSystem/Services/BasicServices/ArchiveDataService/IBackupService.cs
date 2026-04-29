using HrManagementSystem.Contracts.BasicContracts.ArchiveData;

namespace HrManagementSystem.Services.BasicServices.ArchiveDataService
{
    public interface IBackupService
    {
        Task<IEnumerable<BackupResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<BackupResponse>>? GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<BackupResponse>> CreateBackupAsync(string description, CancellationToken cancellationToken);
        Task<Result> RestoreBackupAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> DeleteOldBackupsAsync(int intervalValue, IntervalType intervalType, CancellationToken cancellationToken);
    }
}
