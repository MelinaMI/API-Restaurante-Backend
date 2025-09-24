using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Mapper;
using Application.Models.Request;
using Application.Models.Response;

namespace Application.UseCase.OrderService
{
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IUpdateOrderValidation _updateOrderValidation;
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;
        private readonly ICreateOrderItemService _createOrderItemService;
        private readonly IOrderMapper _orderMapper;
        public UpdateOrderService(IUpdateOrderValidation updateOrderValidation, IOrderQuery orderQuery, IDishQuery dishQuery,ICreateOrderItemService createOrderItemService, IOrderMapper orderMapper)
        {
            _updateOrderValidation = updateOrderValidation;
            _orderQuery = orderQuery;
            _dishQuery = dishQuery;
            _createOrderItemService = createOrderItemService;
            _orderMapper = orderMapper;
        }
        public async Task<OrderUpdateResponse> UpdateOrderAsync(long id, OrderUpdateRequest request)
        {
            await _updateOrderValidation.UpdateOrderValidation(id, request.Items);
            var order = await _orderQuery.GetOrderByIdAsync(id);
            var newItemRequests = new List<Items>(); // Lista de items nuevos a agregar

            foreach (var item in request.Items)
            {
                var dish = await _dishQuery.GetDishByIdAsync(item.Id);                
                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.Dish == item.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.Notes = item.Notes;
                }
                else
                 newItemRequests.Add(item); 
                
            }
            // Creo nuevos ítems si los hay
            var newItems = await _createOrderItemService.CreateItemsAsync(newItemRequests);
            foreach (var newItem in newItems.Items)
                order.OrderItems.Add(newItem);

            var updatedTotal = order.OrderItems.Sum(oi => oi.Quantity * oi.DishNavigation.Price);

            order.Price = updatedTotal;
            order.UpdateDate = DateTime.UtcNow; 
            await _orderQuery.OrderUpdateAsync(order);
            return  _orderMapper.ToOrderUpdateResponse(order); 
           
        }
    }
}
