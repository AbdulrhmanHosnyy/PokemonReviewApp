namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        bool Create(Review review);
        bool DeleteReviews(List<Review> reviews);

    }
}
