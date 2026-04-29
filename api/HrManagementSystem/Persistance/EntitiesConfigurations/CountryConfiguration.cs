using HrManagementSystem.Entities.ApplicationEntities.GeographicDetails;

namespace HrManagementSystem.Persistance.EntitiesConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Indexes
        builder.HasIndex(c => c.NameAr).IsUnique();
        builder.HasIndex(c => c.NameEn).IsUnique();
        builder.HasIndex(c => c.Alpha2Code).IsUnique();
        builder.HasIndex(c => c.Alpha3Code).IsUnique();

        // Properties
        builder.Property(c => c.NameEn) .IsRequired().HasMaxLength(100);

        //builder.ToTable(tb =>
        //           tb.HasCheckConstraint("CHK_Category_NameEn_EnglishOnly", "[NameEn] NOT LIKE '%[^A-Za-z ]%'"));


        //builder.Property(c => c.NameAr).IsRequired().HasMaxLength(100);
        //builder.ToTable(tb =>
        //          tb.HasCheckConstraint("CHK_Category_NameAr_ArabicOnly", "[NameAr] NOT LIKE N'%[^Á-í ]%' COLLATE Arabic_CI_AS"));

        builder.Property(c => c.Alpha2Code)
            .HasMaxLength(2);

        builder.Property(c => c.Alpha3Code)
                .HasMaxLength(3);

        builder.Property(c => c.PhoneCode)
                .HasMaxLength(10);

        builder.Property(c => c.CurrencyCode)
                .HasMaxLength(3);

        // Relationships
        builder.HasMany(c => c.States)         // Country has many States
               .WithOne(s => s.Country)        // State has one Country
               .HasForeignKey(s => s.CountryId)
               .IsRequired(true);
    }
}