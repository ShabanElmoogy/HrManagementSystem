namespace HrManagementSystem.Contracts.BasicContracts.EntityChangeLogs
{
    public record EntityChangeLogsResponse
    (
        int ChangeLogId,
        string EntityName,
        string Key,
        string OldValue,
        string NewValue,
        string ChangedBy,
        DateTime ChangedAt,
        string ChangedByPc
    );


}
