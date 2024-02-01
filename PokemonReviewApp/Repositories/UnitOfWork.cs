namespace PokemonReviewApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository Categories { get; private set; }

        public ICountryRepository Countries { get; private set; }

        public IOwnerRepository Owners { get; private set; }

        public IPokemonRepository Pokemons { get; private set; }

        public IReviewerRepository Reviewers { get; private set; }

        public IReviewRepository Reviews { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Countries = new CountryRepository(_context);
            Owners = new OwnerRepository(_context);
            Pokemons = new PokemonRepository(_context);
            Reviewers = new ReviewerRepository(_context);
            Reviews = new ReviewRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
