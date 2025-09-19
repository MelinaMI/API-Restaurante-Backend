using Application.Interfaces.IOrderItem;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class OrderItemQuery : IOrderItemQuery
    {
        private readonly AppDbContext _context;
        public OrderItemQuery(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<OrderItem?> GetOrderItemByIdAsync(long itemId)
        {
            var item = await _context.OrderItems.Include(oi => oi.StatusNavigation).Include(oi => oi.DishNavigation).FirstOrDefaultAsync(oi => oi.OrderItemId == itemId);
            return item;
        }

        public async Task UpdateOrderItemAsync(OrderItem item)
        {
            _context.OrderItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<OrderItem>> GetItemsByOrderIdAsync(long orderId)
        {
            return await _context.OrderItems
             .Include(oi => oi.DishNavigation)
             .Include(oi => oi.StatusNavigation)
             .Where(oi => oi.OrderItemId == orderId)
             .ToListAsync();
        }

        public Task<IReadOnlyList<OrderItem>> GetItemsByStatusAsync(int statusId)
        {
            throw new NotImplementedException();
        }
    }
}
