using Application.Interfaces.IOrderItem;
using Domain.Entities;
using Infrastructure.Persistence;
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
        public Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderItem>> GetItemsByStatusAsync(int statusId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem?> GetOrderItemByIdAsync(long itemId)
        {
            throw new NotImplementedException();
        }
    }
}
