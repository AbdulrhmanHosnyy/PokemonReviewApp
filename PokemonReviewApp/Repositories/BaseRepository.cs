using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Linq.Expressions;

namespace PokemonReviewApp.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context) 
        {
            _context = context;
        }

        public ICollection<T> GetAll() => _context.Set<T>().ToList();
       
        public T GetById(int id) => _context.Set<T>().Find(id);

        public bool IsExists(int id) => GetById(id) != null;

        public bool Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        
    }
}
