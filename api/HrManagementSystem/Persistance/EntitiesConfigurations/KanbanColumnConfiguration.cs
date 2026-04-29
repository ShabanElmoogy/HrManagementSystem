namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanColumnConfiguration : IEntityTypeConfiguration<KanbanColumn>
{
    public void Configure(EntityTypeBuilder<KanbanColumn> builder)
    {
        builder.ToTable("KanbanColumns");

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Order)
               .IsRequired();

        builder.Property(x => x.IsArchived)
               .HasDefaultValue(false);

        // Unique column name per board
        builder.HasIndex(x => new { x.KanbanBoardId, x.Name })
               .IsUnique();

        builder.HasOne(c => c.KanbanBoard)
               .WithMany(b => b.Columns)
               .HasForeignKey(c => c.KanbanBoardId);

        // Tasks relationship (BoardTask has optional KanbanColumnId)
        builder.HasMany(c => c.Tasks)
               .WithOne(t => t.KanbanColumn)
               .HasForeignKey(t => t.KanbanColumnId);

        // Cards relationship configured primarily on KanbanCardConfiguration, but safe to declare here as well
        builder.HasMany(c => c.Cards)
               .WithOne(card => card.KanbanColumn)
               .HasForeignKey(card => card.KanbanColumnId);
    }
}