namespace HrManagementSystem.Contracts.BasicContracts.ReportMaster
{
    public record ReportMasterRequest(
        string ReportName,
        string ExportedName,
        string ReportPath,
        string Logo,
        string ViewName,
        int ReportCategoryId);
}
