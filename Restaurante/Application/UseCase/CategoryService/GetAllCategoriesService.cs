using Application.Interfaces.ICategory;
using Domain.Entities;

namespace Application.UseCase.CategoryService
{
    public class GetAllCategoriesService : IGetAllCategories
    {
        private readonly ICategoryQuery _categoryQuery;

        public GetAllCategoriesService(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }
        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            
            var categ = await _categoryQuery.GetAllCategoriesAsync();
            return (IReadOnlyList<Category>)categ.Select(c => c.Name).ToList();
        }
    }
}
