using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder
{
    public interface IOrderQuery
    {
        Task<Order?> GetOrderByIdAsync(long orderId);
        IQueryable<Order> GetAllOrders();
        Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(int statusId);
        Task <Order> OrderUpdateAsync(Order order);
    }
}
