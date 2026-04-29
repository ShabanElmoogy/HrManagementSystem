namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class KanbanCardAssigneeConfiguration : IEntityTypeConfiguration<KanbanCardAssignee>
{
    public void Configure(EntityTypeBuilder<KanbanCardAssignee> builder)
    {
        builder.HasIndex(x => new { x.KanbanCardId, x.UserId }).IsUnique();

        builder.HasOne(a => a.KanbanCard)
               .WithMany(c => c.Assignees)
               .HasForeignKey(a => a.KanbanCardId);

        builder.HasOne(a => a.User)
               .WithMany()
               .HasForeignKey(a => a.UserId);
    }
}
