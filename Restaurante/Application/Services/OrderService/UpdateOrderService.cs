using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.Services.OrderService
{
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IUpdateOrderValidation _updateOrderValidation;
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;
        private readonly ICreateOrderItem _createOrderItemService;

        public UpdateOrderService(IUpdateOrderValidation updateOrderValidation, IOrderQuery orderQuery, IDishQuery dishQuery,ICreateOrderItem createOrderItemService)
        {
            _updateOrderValidation = updateOrderValidation;
            _orderQuery = orderQuery;
            _dishQuery = dishQuery;
            _createOrderItemService = createOrderItemService;
        }

        public async Task<OrderUpdateResponse> UpdateOrder(long id, OrderUpdateRequest request)
        {
            // Validar la orden y los items
            await _updateOrderValidation.UpdateOrderValidation(id, request.Items);
            // Obtener la orden existente
            var order = await _orderQuery.GetOrderByIdAsync(id);
            if (order == null)
                throw new NotFoundException("La orden no existe.");

            var newItemRequests = new List<Items>(); // Ítems nuevos a agregar

            // Actualizar o agregar ítems
            foreach (var item in request.Items)
            {
                var dish = await _dishQuery.GetDishByIdAsync(item.Id);
                if (dish == null || !dish.Available)
                    throw new BadRequestException($"El plato '{item.Id}' no está disponible.");

                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.Dish == item.Id);

                if (existingItem != null)
                {
                    // Actualizo cantidad y notas
                    existingItem.Quantity = item.Quantity;
                    existingItem.Notes = item.Notes;
                }
                else
                {
                    // Este es un ítem nuevo que hay que crear
                    newItemRequests.Add(item);
                }
            }
            // Crear nuevos ítems si los hay
            var (newItems, _) = await _createOrderItemService.CreateItemsAsync(newItemRequests);
            
            foreach (var newItem in newItems)
            {
                order.OrderItems.Add(newItem);
            }
            await _orderQuery.OrderUpdateAsync(order);
            // Recalcular el total
            var total = order.OrderItems.Sum(oi => oi.Quantity * oi.DishNavigation.Price);
            
            var updatedOrder = new OrderUpdateResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = (double)total,
                UpdateAt = order.UpdateDate
            };
            return updatedOrder;
        }
    }
}
