using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Persistance.EntitiesConfigurations
{
    public class BackupConfiguration : IEntityTypeConfiguration<Backup>
    {
        public void Configure(EntityTypeBuilder<Backup> builder)
        {
            builder.Property(b => b.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(b => b.DatabaseName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(b => b.BackupPath)
                    .IsRequired()
                    .HasMaxLength(500);

            builder.Property(b => b.Description)
                    .HasMaxLength(500);

            builder.Property(b => b.Status)
                    .IsRequired()
                    .HasMaxLength(50);

            // Create an index on FileName for faster lookups
            builder.HasIndex(b => b.FileName);

        }
    }
}
