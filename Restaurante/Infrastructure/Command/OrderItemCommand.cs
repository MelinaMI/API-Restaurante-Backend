using Application.Interfaces.IOrderItem;
using Domain.Entities;
using Infrastructure.Persistence;
namespace Infrastructure.Command
{
    public class OrderItemCommand : IOrderItemCommand
    {
        private readonly AppDbContext _context;
        public OrderItemCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<long> InsertOrderItemAsync(OrderItem item)
        {
            _context.Set<OrderItem>().Add(item);
            await _context.SaveChangesAsync();
            return item.OrderItemId;
        }
        public async Task UpdateOrderItemAsync(OrderItem item)
        {
            _context.Set<OrderItem>().Update(item);
            await _context.SaveChangesAsync();
        }

    }
}
