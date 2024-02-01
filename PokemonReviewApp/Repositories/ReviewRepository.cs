namespace PokemonReviewApp.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId) =>
            _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();

        public bool Create(Review review)
        {
            _context.Add(review);
            return true;
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return true;
        }
    }
}
