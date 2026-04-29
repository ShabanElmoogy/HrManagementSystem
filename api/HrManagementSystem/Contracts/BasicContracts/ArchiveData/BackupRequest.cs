namespace HrManagementSystem.Contracts.BasicContracts.ArchiveData
{
    public record BackupRequest(
        int Id,
        string FileName,
        string DatabaseName,
        string BackupPath,
        string Description,
        string Status
    );
}
