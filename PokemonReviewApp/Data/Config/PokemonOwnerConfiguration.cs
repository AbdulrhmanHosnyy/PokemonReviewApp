using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class PokemonOwnerConfiguration : IEntityTypeConfiguration<PokemonOwner>
    {
        public void Configure(EntityTypeBuilder<PokemonOwner> builder)
        {
            builder.HasKey(pc => new { pc.PokemonId, pc.OwnerId });
            builder.HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonOwners).HasForeignKey(p => p.PokemonId);
            builder.HasOne(p => p.Owner).WithMany(pc => pc.PokemonOwners).HasForeignKey(c => c.OwnerId);
            builder.ToTable("PokemonOwners");
        }
    }
}
