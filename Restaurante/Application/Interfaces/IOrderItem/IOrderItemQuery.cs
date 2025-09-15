using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrderItem
{
    public interface IOrderItemQuery
    {
        Task<OrderItem?> GetOrderItemByIdAsync(long itemId);
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(long orderId);
        Task<IEnumerable<OrderItem>> GetItemsByStatusAsync(int statusId);
    }

}
