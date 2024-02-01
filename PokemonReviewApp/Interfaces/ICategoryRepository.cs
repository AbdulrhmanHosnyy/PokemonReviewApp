namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool Create(Category category);
    }
}
