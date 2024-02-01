using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.FirstName).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.Property(o => o.LastName).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.Property(o => o.Gym).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.HasOne(e => e.Country).WithMany(e => e.Owners).HasForeignKey("CountryId");
            builder.ToTable("Owners");
        }
    }
}
