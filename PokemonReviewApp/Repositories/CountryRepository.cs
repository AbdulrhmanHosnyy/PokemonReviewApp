using PokemonReviewApp.Models;
using System.Linq.Expressions;

namespace PokemonReviewApp.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
       
        public Country GetCountryByOwner(int ownerId) =>
            _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();

        public ICollection<Owner> GetOwnersByCountry(int countryId) =>
            _context.Owners.Where(o => o.Country.Id == countryId).ToList();

        public bool Create(Country country)
        {
            _context.Add(country);
            return true;
        }

    }
}
