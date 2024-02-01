using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class PokemonConfiguration : IEntityTypeConfiguration<Pokemon>
    {
        public void Configure(EntityTypeBuilder<Pokemon> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.ToTable("Pokemons");
        }
    }
}
