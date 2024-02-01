using System.Linq.Expressions;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool Create(Country country);
    }
}
