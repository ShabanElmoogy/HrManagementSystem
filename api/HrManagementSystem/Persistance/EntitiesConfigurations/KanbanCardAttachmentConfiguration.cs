namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanCardAttachmentConfiguration : IEntityTypeConfiguration<KanbanCardAttachment>
{
    public void Configure(EntityTypeBuilder<KanbanCardAttachment> builder)
    {
        builder.HasOne(a => a.KanbanCard)
               .WithMany(c => c.Attachments)
               .HasForeignKey(a => a.KanbanCardId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.UploadedFile)
               .WithMany()
               .HasForeignKey(a => a.UploadedFileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.KanbanCardId);
        builder.HasIndex(a => a.UploadedFileId);
    }
}
