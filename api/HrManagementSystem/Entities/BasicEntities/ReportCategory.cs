namespace HrManagementSystem.Entities.BasicEntities
{
    public class ReportCategory : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ReportMaster>? ReportMasters { get; set; }
    }
}
