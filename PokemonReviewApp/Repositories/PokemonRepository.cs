using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Linq.Expressions;

namespace PokemonReviewApp.Repositories
{
    public class PokemonRepository : BaseRepository<Pokemon>, IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
       
        public Pokemon Find(Expression<Func<Pokemon, bool>> criteria) => _context.Pokemons.SingleOrDefault(criteria);
        public decimal GetPokemonRating(int id)
        {
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == id);
            if (reviews.Count() == 0)
                return 0;
            return ((decimal) reviews.Sum(r => r.Rating) / reviews.Count());
        }
        public bool Create(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.SingleOrDefault(o => o.Id == ownerId);
            var category = _context.Categories.SingleOrDefault(c => c.Id == categoryId);

            if(owner == null || category == null) return false;

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon,
            };
            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };
            _context.Add(pokemonCategory);

            _context.Add(pokemon);
            return true;    
        }

    }
}
