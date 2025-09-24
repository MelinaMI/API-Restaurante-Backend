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
        public async Task DeleteDishAsync(Dish dish)
        {
            dish.Available = false; // Soft delete
            dish.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
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
