namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanLabelConfiguration : IEntityTypeConfiguration<KanbanLabel>
{
    public void Configure(EntityTypeBuilder<KanbanLabel> builder)
    {
        builder.ToTable("KanbanLabels");

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.ColorHex)
               .HasMaxLength(7) // e.g., #FFFFFF
               .IsRequired();

        // Unique label name per board
        builder.HasIndex(x => new { x.KanbanBoardId, x.Name })
               .IsUnique();

        builder.HasOne(l => l.KanbanBoard)
               .WithMany(b => b.Labels)
               .HasForeignKey(l => l.KanbanBoardId);

        builder.HasMany(l => l.CardLabels)
               .WithOne(cl => cl.KanbanLabel)
               .HasForeignKey(cl => cl.KanbanLabelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}