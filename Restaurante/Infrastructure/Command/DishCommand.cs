using Application.Interfaces.IDish;
using Domain.Entities;
using Infrastructure.Persistence;
using System;

namespace Infrastructure.Commands
{
    public class DishCommand : IDishCommand
    {
        private readonly AppDbContext _context;
        public DishCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task DeleteDishAsync(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null) 
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertDishAsync(Dish dish)
        {
            _context.Add(dish);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Entry(dish).Property(d => d.Category).IsModified = true;
            _context.Update(dish);
            await _context.SaveChangesAsync();
        }
    }
}
