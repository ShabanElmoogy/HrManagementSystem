namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanBoardMemberConfiguration : IEntityTypeConfiguration<KanbanBoardMember>
{
    public void Configure(EntityTypeBuilder<KanbanBoardMember> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.KanbanBoardId).IsRequired();
        builder.Property(x => x.Role).IsRequired();

        // Composite unique index to prevent duplicate memberships
        builder.HasIndex(x => new { x.KanbanBoardId, x.UserId }).IsUnique();

        builder.HasOne(m => m.KanbanBoard)
               .WithMany(b => b.Members)
               .HasForeignKey(m => m.KanbanBoardId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.User)
               .WithMany()
               .HasForeignKey(m => m.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
