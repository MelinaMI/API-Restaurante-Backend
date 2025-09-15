using Application.Interfaces.IOrder;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId;
        }

        public Task<long> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
