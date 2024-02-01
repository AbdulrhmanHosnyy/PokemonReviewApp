using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.ToTable("Countries");
        }
    }
}
