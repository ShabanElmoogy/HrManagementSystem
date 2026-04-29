namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        // Indexes
        builder.HasIndex(s => new { s.NameAr, s.CountryId }).IsUnique();
        builder.HasIndex(s => new { s.NameEn, s.CountryId }).IsUnique();
        builder.HasIndex(s => new { s.Code, s.CountryId }).IsUnique();

        // Properties - Apply constraints for Arabic and English
        builder.Property(s => s.NameEn)
                .IsRequired()
                .HasMaxLength(100);
        builder.ToTable(tb =>
                   tb.HasCheckConstraint("CHK_State_NameEn_EnglishOnly", "[NameEn] NOT LIKE '%[^A-Za-z ]%'"));

        builder.Property(s => s.NameAr)
                .IsRequired()
                .HasMaxLength(100);
        builder.ToTable(tb =>
                  tb.HasCheckConstraint("CHK_State_NameAr_ArabicOnly", "[NameAr] NOT LIKE N'%[^ء-ي ]%' COLLATE Arabic_CI_AS"));

        builder.Property(s => s.Code)
                .IsRequired()
                .HasMaxLength(10);

        // Relationships
        builder.HasOne(s => s.Country)
               .WithMany(c => c.States)
               .HasForeignKey(s => s.CountryId)
               .IsRequired();

        builder.HasMany(s => s.Districts)
               .WithOne(d => d.State)
               .HasForeignKey(d => d.StateId)
               .IsRequired(false);
    }
}