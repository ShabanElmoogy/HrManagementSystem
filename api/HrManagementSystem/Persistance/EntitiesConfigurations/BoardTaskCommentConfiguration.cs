namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class BoardTaskCommentConfiguration : IEntityTypeConfiguration<BoardTaskComment>
{
    public void Configure(EntityTypeBuilder<BoardTaskComment> builder)
    {
        builder.ToTable("BoardTaskComment");

        builder.Property(c => c.CommentText)
               .HasMaxLength(2000)
               .IsRequired();

        builder.HasOne(c => c.BoardTask)
               .WithMany(t => t.Comments)
               .HasForeignKey(c => c.BoardTaskId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ParentComment)
               .WithMany(c => c.Replies)
               .HasForeignKey(c => c.ParentCommentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.BoardTaskId);
        builder.HasIndex(c => c.ParentCommentId);
        builder.HasIndex(c => c.UserId);
    }
}