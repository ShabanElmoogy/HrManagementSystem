namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanCardLabelConfiguration : IEntityTypeConfiguration<KanbanCardLabel>
{
    public void Configure(EntityTypeBuilder<KanbanCardLabel> builder)
    {
        // Relationships
        builder.HasOne(cl => cl.KanbanCard)
               .WithMany(c => c.CardLabels)
               .HasForeignKey(cl => cl.KanbanCardId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cl => cl.KanbanLabel)
               .WithMany(l => l.CardLabels)
               .HasForeignKey(cl => cl.KanbanLabelId)
               .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(cl => cl.KanbanCardId);
        builder.HasIndex(cl => cl.KanbanLabelId);
        builder.HasIndex(cl => new { cl.KanbanCardId, cl.KanbanLabelId });

        // No additional properties on the join entity beyond audit fields
    }
}
