using Application.Interfaces.IOrder;
using Application.Mapper;
using Application.Models.Response;

namespace Application.Services.OrderService
{
    public class GetOrderByIdService : IGetOrderById
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderMapper _orderMapper;

        public GetOrderByIdService(IOrderQuery orderQuery, IOrderMapper orderMapper)
        {
            _orderQuery = orderQuery;
            _orderMapper = orderMapper;
        }

        public async Task<OrderDetailsResponse> GetOrderByIdAsync(long orderId)
        {
            var order = await _orderQuery.GetOrderByIdAsync(orderId);
            if (order == null)
                return null;

            return _orderMapper.ToDetailsResponse(order);

        }
    }
}
