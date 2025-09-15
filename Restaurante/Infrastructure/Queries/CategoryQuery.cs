using Application.Interfaces.ICategory;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class CategoryQuery : ICategoryQuery
    {
        private readonly AppDbContext _context;
        public CategoryQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public Task<Category> GetCategoryByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}