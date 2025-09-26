using Application.Interfaces.IDish;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;
        public DishQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Dish>> GetAll()
        {
            return await _context.Dishes
                .Include(c => c.CategoryNavigation)
                .AsNoTracking().ToListAsync();
        }
        public async Task<Dish?> GetDishByIdAsync(Guid id)
        {
            return await _context.Dishes
                .Include(c => c.CategoryNavigation)
                .Include(oi=> oi.OrderItems)
                    .ThenInclude(o => o.OrderNavigation)
                .FirstOrDefaultAsync(d => d.DishId == id);
        }
        public async Task<Dish?> GetByNameAsync(string name)
        {
            var normalizedName = name.Trim().ToLowerInvariant();

            return await _context.Dishes
                .Include(d => d.CategoryNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => EF.Functions.Like(d.Name, normalizedName));
        }
    }
}
