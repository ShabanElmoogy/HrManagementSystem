namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanCardCommentConfiguration : IEntityTypeConfiguration<KanbanCardComment>
{
    public void Configure(EntityTypeBuilder<KanbanCardComment> builder)
    {
        builder.Property(c => c.CommentText)
               .HasMaxLength(2000)
               .IsRequired();

        builder.HasOne(c => c.KanbanCard)
               .WithMany(k => k.Comments)
               .HasForeignKey(c => c.KanbanCardId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.KanbanCardId);
    }
}
