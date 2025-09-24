using Application.Interfaces.IOrder;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.UseCase.OrderService
{
    public class GetOrderByIdService : IGetOrderByIdService
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderMapper _orderMapper;
        private readonly IGetOrderByIdValidation _orderValidator;
        public GetOrderByIdService(IOrderQuery orderQuery, IOrderMapper orderMapper, IGetOrderByIdValidation orderValidator)
        {
            _orderQuery = orderQuery;
            _orderMapper = orderMapper;
            _orderValidator = orderValidator;
        }
        public async Task<OrderDetailsResponse> GetOrderByIdAsync(long orderId)
        {
            await _orderValidator.ValidateOrderById(orderId);
            var order = await _orderQuery.GetOrderByIdAsync(orderId);
            return _orderMapper.ToDetailsResponse(order);
        }
    }
}
