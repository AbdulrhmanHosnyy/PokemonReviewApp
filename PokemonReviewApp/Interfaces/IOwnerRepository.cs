namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository : IBaseRepository<Owner>
    {
        ICollection<Owner> GetOwnersOfAPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonsOfAnOwner(int ownerId);
        bool Create(Owner owner);
    }
}
