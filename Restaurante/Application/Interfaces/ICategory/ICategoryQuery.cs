using Domain.Entities;

namespace Application.Interfaces.ICategory
{
    public interface ICategoryQuery
    {
        Task<Category> GetByCategoryIdAsync(int categoryId);
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByNameAsync(string name);
    }
}
