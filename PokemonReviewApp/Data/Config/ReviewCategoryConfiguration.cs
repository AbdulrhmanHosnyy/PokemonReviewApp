using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonReviewApp.Data.Config
{
    public class ReviewCategoryConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Title).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.Property(r => r.Text).HasColumnType("VARCHAR").HasMaxLength(255);
            builder.HasOne(e => e.Reviewer).WithMany(e => e.Reviews).HasForeignKey("ReviewerId"); 
            builder.HasOne(e => e.Pokemon).WithMany(e => e.Reviews).HasForeignKey("PokemonId");
            builder.ToTable("Reviews");
        }
    }
}
