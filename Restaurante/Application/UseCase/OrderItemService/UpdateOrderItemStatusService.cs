using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.UseCase.OrderItemService
{
    public class UpdateOrderItemStatusService : IUpdateOrderItemStatusService
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IGetOrderByIdValidation _getOrderByIdValidation;
        private readonly IOrderItemCommand _orderItemCommand;
        private readonly IUpdateOrderItemStatusValidation _statusValidator;
        private readonly IUpdateOrderStatusService _updateOrderStatusService;
        private readonly IOrderMapper _orderMapper;
        private readonly IOrderCommand _orderCommand;
        public UpdateOrderItemStatusService(IOrderQuery orderQuery, IOrderItemCommand orderItemCommand, IGetOrderByIdValidation getOrderByIdValidation, IUpdateOrderItemStatusValidation statusValidator, IUpdateOrderStatusService updateOrderStatusService, IOrderMapper orderMapper, IOrderCommand orderCommand)
        {
            _orderQuery = orderQuery;
            _orderItemCommand = orderItemCommand;
            _getOrderByIdValidation = getOrderByIdValidation;
            _statusValidator = statusValidator;
            _updateOrderStatusService = updateOrderStatusService;
            _orderMapper = orderMapper;
            _orderCommand = orderCommand;
        }
        public async Task<OrderUpdateResponse> UpdateOrderItemStatusAsync(long orderId, long itemId, int newStatusId)
        {
            await _getOrderByIdValidation.ValidateOrderById(orderId);

            var order = await _orderQuery.GetOrderByIdAsync(orderId);
            var item = order.OrderItems.FirstOrDefault(i => i.OrderItemId == itemId);

            if (item == null)
                throw new NotFoundException("Item no encontrado en la orden");

            await _statusValidator.ValidateUpdateOrderItemStatusAsync(item.Status, newStatusId);

            item.Status = newStatusId;          
            await _orderItemCommand.UpdateOrderItemAsync(item);

            await _updateOrderStatusService.UpdateOrderStatusBasedOnItemsAsync(order);

            item.CreateDate = DateTime.UtcNow;
            await _orderCommand.OrderUpdateAsync(order);

           return _orderMapper.ToOrderUpdateResponse(order);
        }
    }
}
