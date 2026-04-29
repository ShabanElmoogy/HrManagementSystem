using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Persistance.EntitiesConfigurations
{
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.Property(x => x.FileName).HasMaxLength(250);
            builder.Property(x => x.StoredFileName).HasMaxLength(250);
            builder.Property(x => x.ContentType).HasMaxLength(250);
            builder.Property(x => x.FileExtension).HasMaxLength(10);
        }
    }
}