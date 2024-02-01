using System.Linq.Expressions;

namespace PokemonReviewApp.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        ICollection<T> GetAll();
        T GetById(int id);
        bool IsExists(int id);
        bool Update(T entity);
        bool Delete(T entity);    
    }
}
