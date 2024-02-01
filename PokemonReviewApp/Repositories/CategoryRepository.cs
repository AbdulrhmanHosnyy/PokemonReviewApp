namespace PokemonReviewApp.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId) => 
            _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.Pokemon).ToList();
        public bool Create(Category category)
        {
            _context.Add(category);
            return true;
        }

    }
}
