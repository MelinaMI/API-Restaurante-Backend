using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrderItem
{
    public interface IUpdateOrderItemStatusService
    {
        Task<OrderUpdateResponse> UpdateOrderItemStatusAsync(long orderId, long itemId, int newStatusId);
    }
}
