using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;

namespace Application.UseCase.OrderService
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly ICreateOrderValidation _validation;
        private readonly IOrderCommand _orderCommand;
        private readonly ICreateOrderItemService _createOrderItemService;
        private readonly IOrderMapper _orderMapper;
        public CreateOrderService(ICreateOrderValidation validation, IOrderCommand orderCommand, ICreateOrderItemService createOrderItemService, IOrderMapper orderMapper)
        {
            _validation = validation;
            _orderCommand = orderCommand;
            _createOrderItemService = createOrderItemService;
            _orderMapper = orderMapper;
        }
        public async Task<OrderCreateResponse> CreateOrderAsync(OrderRequest request)
        {
            await _validation.ValidateOrderAsync(request);

            var (items, total) = await _createOrderItemService.CreateItemsAsync(request.Items);

            var order = new Order
            {
                DeliveryTo = request.Delivery.To,
                Notes = request.Notes,
                Price = total,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                DeliveryType = request.Delivery.Id,
                OverallStatus = 1, //Pending
                OrderItems = items
            };   
            long orderId = await _orderCommand.InsertOrderAsync(order);
            return _orderMapper.ToOrderCreateResponse(order);
        }
    }
}
