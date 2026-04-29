namespace HrManagementSystem.Entities.BasicEntities
{
    public class Backup : AuditableEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BackupPath { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public long Size { get; set; }

        public string Status { get; set; } = null!; // "Completed", "Failed", "Deleted", "File Missing"
    }
}
