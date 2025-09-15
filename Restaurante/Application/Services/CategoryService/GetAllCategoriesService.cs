using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Domain.Entities;
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
        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            
            var categ = await _categoryQuery.GetAllCategoriesAsync();
            return (IReadOnlyList<Category>)categ.Select(c => c.Name).ToList();
        }
    }
}
