using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class ReviewerRepository : BaseRepository<Reviewer>, IReviewerRepository
    {
        private readonly AppDbContext _context;
        public ReviewerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Review> GetReviewsByAReviewer(int reviewerId) =>
            _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();

        public bool Create(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return true;
        }

    }
}
