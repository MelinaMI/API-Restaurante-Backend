using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderService
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly ICreateOrderValidation _validation;
        private readonly IDishQuery _dishQuery;
        private readonly IOrderCommand _orderCommand;
        private readonly IOrderItemCommand _orderItemCommand;
        public CreateOrderService(ICreateOrderValidation validation, IDishQuery dishQuery, IOrderCommand orderCommand, IOrderItemCommand orderItemCommand)
        {
            _validation = validation;
            _dishQuery = dishQuery;
            _orderCommand = orderCommand;
            _orderItemCommand = orderItemCommand;
        }
        public async Task<long> CreateOrderAsync(OrderRequest request)
        {
            await _validation.ValidateOrderAsync(request);
            decimal total = 0;
            var items = new List<OrderItem>();

            foreach (var itemRequest in request.Items)
            {
                var dish = await _dishQuery.GetDishByIdAsync(itemRequest.Id);
                if (dish == null || !dish.Available)
                    throw new Exception($"Plato inválido o inactivo: {itemRequest.Id}");
                if (itemRequest.Quantity <= 0)
                    throw new Exception("Cantidad debe ser mayor a cero");
                total += dish.Price * itemRequest.Quantity;

                items.Add(new OrderItem
                {
                    Dish = dish.DishId,
                    Quantity = itemRequest.Quantity,
                    Notes = itemRequest.Notes,
                    CreateDate = DateTime.UtcNow,
                    Status = 1 // Estado inicial: Pending
                });
            }
            var order = new Order
            {
                DeliveryTo = request.Delivery.To,
                Notes = request.Notes,
                Price = total,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                DeliveryType = request.Delivery.Id,
                OrderItems = items,
                OverallStatus = 1 // Estado inicial: Pending
            };
            long orderId = await _orderCommand.InsertOrderAsync(order);

            foreach (var item in items)
            {
                item.Order = orderId;
                await _orderItemCommand.InsertOrderItemAsync(item);
            }

            return orderId;
        }
    }
}
