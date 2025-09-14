using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CategoryService
{
    public class GetAllCategoriesService : IGetAllCategories
    {
        private readonly ICategoryQuery _categoryQuery;

        public GetAllCategoriesService(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }
        public async Task<IReadOnlyList<string>> GetAllCategoriesAsync()
        {
            
            var categ = await _categoryQuery.GetAllCategoriesAsync();
            return categ.Select(c => c.Name).ToList();
        }
    }
}
