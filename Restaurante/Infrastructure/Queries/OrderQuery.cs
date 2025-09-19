using Application.Interfaces.IOrder;
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
    public class OrderQuery : IOrderQuery
    {
        private readonly AppDbContext _context;
        public OrderQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> GetOrderByIdAsync(long orderId)
        {
            return await _context.Orders
           .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.DishNavigation)
           .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.StatusNavigation)
           .Include(o => o.DeliveryTypeNavigation)
           .Include(o => o.OverallStatusNavigation)
           .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }


        public IQueryable<Order> GetAllOrders()
        {
            return _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.DishNavigation)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.StatusNavigation)
            .Include(o => o.DeliveryTypeNavigation)
            .Include(o => o.OverallStatusNavigation);
        }

        public Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(int statusId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> OrderUpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
