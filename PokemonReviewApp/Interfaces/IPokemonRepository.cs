using System.Linq.Expressions;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository : IBaseRepository<Pokemon>
    {
        Pokemon Find(Expression<Func<Pokemon, bool>> criteria);
        decimal GetPokemonRating(int id);
        bool Create(int ownerId, int categoryId, Pokemon pokemon);
    }
}
