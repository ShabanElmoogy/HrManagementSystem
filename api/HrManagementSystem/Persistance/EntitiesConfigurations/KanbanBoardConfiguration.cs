namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanBoardConfiguration : IEntityTypeConfiguration<KanbanBoard>
{
    public void Configure(EntityTypeBuilder<KanbanBoard> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasMany(b => b.Columns)
               .WithOne(c => c.KanbanBoard)
               .HasForeignKey(c => c.KanbanBoardId);

        builder.HasMany(b => b.Members)
                .WithOne(m => m.KanbanBoard)
                .HasForeignKey(m => m.KanbanBoardId);


        builder.HasMany(b => b.Labels)
                .WithOne(l => l.KanbanBoard)
                .HasForeignKey(l => l.KanbanBoardId);

        builder.HasMany(b => b.Tasks)
                .WithOne(t => t.KanbanBoard)
                .HasForeignKey(t => t.KanbanBoardId);

    }
}