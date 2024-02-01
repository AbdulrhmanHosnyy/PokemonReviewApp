using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class PokemonCategoryConfiguration : IEntityTypeConfiguration<PokemonCategory>
    {
        public void Configure(EntityTypeBuilder<PokemonCategory> builder)
        {
            builder.HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            builder.HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonCategories).HasForeignKey(p => p.PokemonId);
            builder.HasOne(p => p.Category).WithMany(pc => pc.PokemonCategories).HasForeignKey(c => c.CategoryId);
            builder.ToTable("PokemonCategories");
        }
    }
}
