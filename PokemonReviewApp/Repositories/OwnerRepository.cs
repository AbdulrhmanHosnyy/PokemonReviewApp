using PokemonReviewApp.Models;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Repositories
{
    public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
    {
        private readonly AppDbContext _context;
        public OwnerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokemonId) =>
            _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).Select(po => po.Owner).ToList();
        

        public ICollection<Pokemon> GetPokemonsOfAnOwner(int ownerId) =>
            _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();

        public bool Create(Owner owner)
        {
            _context.Add(owner);
            return true;
        }

    }
}
