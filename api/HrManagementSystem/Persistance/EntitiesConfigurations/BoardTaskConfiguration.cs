namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class BoardTaskConfiguration : IEntityTypeConfiguration<BoardTask>
{
    public void Configure(EntityTypeBuilder<BoardTask> builder)
    {
        builder.ToTable("BoardTasks");

        builder.Property(x => x.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(x => x.Priority)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(x => x.Position)
               .IsRequired();

        // Optional Board relation
        builder.HasOne(t => t.KanbanBoard)
               .WithMany(b => b.Tasks)
               .HasForeignKey(t => t.KanbanBoardId);

        // Optional Column relation
        builder.HasOne(t => t.KanbanColumn)
               .WithMany(c => c.Tasks)
               .HasForeignKey(t => t.KanbanColumnId);

        // Optional Assignee relation
        builder.HasOne(t => t.Assignee)
               .WithMany()
               .HasForeignKey(t => t.AssigneeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Comments
        builder.HasMany(t => t.Comments)
               .WithOne(c => c.BoardTask)
               .HasForeignKey(c => c.BoardTaskId)
               .OnDelete(DeleteBehavior.Restrict);

        // Attachments
        builder.HasMany(t => t.Attachments)
               .WithOne(a => a.BoardTask)
               .HasForeignKey(a => a.BoardTaskId)
               .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.KanbanBoardId);
        builder.HasIndex(x => x.KanbanColumnId);
        builder.HasIndex(x => x.AssigneeId);
        builder.HasIndex(x => new { x.KanbanBoardId, x.KanbanColumnId, x.Position });
    }
}