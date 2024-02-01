using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class ReviewerConfiguration : IEntityTypeConfiguration<Reviewer>
    {
        public void Configure(EntityTypeBuilder<Reviewer> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.FirstName).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.Property(r => r.LastName).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.ToTable("Reviewers");
        }
    }
}
