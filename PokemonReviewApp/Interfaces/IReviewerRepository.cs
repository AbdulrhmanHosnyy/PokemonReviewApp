namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository : IBaseRepository<Reviewer>
    {
        ICollection<Review> GetReviewsByAReviewer(int reviewerId);
        bool Create(Reviewer reviewer);
    }
}
