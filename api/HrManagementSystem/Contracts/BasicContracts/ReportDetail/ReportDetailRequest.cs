namespace HrManagementSystem.Contracts.BasicContracts.ReportDetail
{
    public record ReportDetailRequest(int Id, string PropertyName, string ColumnName, int ReportMasterId);
}
