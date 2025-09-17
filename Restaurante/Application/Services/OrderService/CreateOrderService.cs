using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Response;
using Application.Services.OrderItemService;
using Domain.Entities;
using static Application.Validators.Exceptions;

namespace Application.Services.OrderService
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly ICreateOrderValidation _validation;
        private readonly IDishQuery _dishQuery;
        private readonly IOrderCommand _orderCommand;
        private readonly IOrderItemCommand _orderItemCommand;
        private readonly ICreateOrderItem _createOrderItemService;
        public CreateOrderService(ICreateOrderValidation validation, IDishQuery dishQuery, IOrderCommand orderCommand, IOrderItemCommand orderItemCommand, ICreateOrderItem createOrderItemService)
        {
            _validation = validation;
            _dishQuery = dishQuery;
            _orderCommand = orderCommand;
            _orderItemCommand = orderItemCommand;
            _createOrderItemService = createOrderItemService;
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

            // Insertar la orden **con sus items en EF Core**
            long orderId = await _orderCommand.InsertOrderWithItemsAsync(order);

            var newOrder = new OrderCreateResponse
            {
                OrderNumber = orderId,
                TotalAmount = (double)total,
                CreatedAt = order.CreateDate
            };
            return newOrder;
        }
    }
}
