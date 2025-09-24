using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces.IOrder
{
    public interface IUpdateOrderService
    {
        Task<OrderUpdateResponse> UpdateOrderAsync(long id,OrderUpdateRequest request);
    }
}
