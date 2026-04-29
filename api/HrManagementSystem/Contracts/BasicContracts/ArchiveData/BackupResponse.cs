namespace HrManagementSystem.Contracts.BasicContracts.ArchiveData
{
    public record BackupResponse(
        int Id,
        string FileName,
        string DatabaseName,
        string BackupPath,
        string Description,
        long Size,
        string Status,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted);
}
