namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class BoardTaskAttachmentConfiguration : IEntityTypeConfiguration<BoardTaskAttachment>
{
    public void Configure(EntityTypeBuilder<BoardTaskAttachment> builder)
    {
        builder.ToTable("BoardTaskAttachment");

        builder.HasOne(a => a.BoardTask)
               .WithMany(t => t.Attachments)
               .HasForeignKey(a => a.BoardTaskId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.UploadedFile)
               .WithMany()
               .HasForeignKey(a => a.UploadedFileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.BoardTaskId);
        builder.HasIndex(a => a.UploadedFileId);
    }
}