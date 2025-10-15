using Application.Interfaces.IOrder;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Command
{
    public class OrderCommand : IOrderCommand
    {
        private readonly AppDbContext _context;
        public OrderCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<long> InsertOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.OrderId;
        }
        public async Task<Order> OrderUpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
