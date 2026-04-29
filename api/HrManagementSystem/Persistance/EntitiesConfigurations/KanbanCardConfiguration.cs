namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanCardConfiguration : IEntityTypeConfiguration<KanbanCard>
{
    public void Configure(EntityTypeBuilder<KanbanCard> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);

        builder.HasOne(c => c.KanbanColumn)
               .WithMany(col => col.Cards)
               .HasForeignKey(c => c.KanbanColumnId);

        builder.HasMany(c => c.Assignees)
               .WithOne(a => a.KanbanCard)
               .HasForeignKey(a => a.KanbanCardId);

        builder.HasMany(c => c.CardLabels)
               .WithOne(cl => cl.KanbanCard)
               .HasForeignKey(cl => cl.KanbanCardId);

        builder.HasMany(c => c.Comments)
               .WithOne(com => com.KanbanCard)
               .HasForeignKey(com => com.KanbanCardId);

        builder.HasMany(c => c.Attachments)
               .WithOne(att => att.KanbanCard)
               .HasForeignKey(att => att.KanbanCardId);
    }
}
