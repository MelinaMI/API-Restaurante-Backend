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
        Task<IReadOnlyList<Order>> GetAllOrders();
        Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(int statusId);
    }
}
