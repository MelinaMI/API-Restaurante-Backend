using Application.Interfaces.IDish;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Queries
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;
        public DishQuery(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Dish> GetAll()
        {
            return _context.Dishes
                .Include(c => c.CategoryNavigation)
                .AsNoTracking();
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
                .FirstOrDefaultAsync(d => d.Name.ToLower() == normalizedName);
        }

        public async Task<decimal> GetDishPriceAsync(Guid id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.DishId == id);
            if (dish == null || !dish.Available)
                throw new Exception("Plato no disponible");
            return dish.Price;
        }
    }
}
