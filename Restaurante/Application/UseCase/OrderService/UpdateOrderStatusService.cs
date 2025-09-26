using Application.Interfaces.IOrder;
using Domain.Entities;

namespace Application.UseCase.OrderService
{
    public class UpdateOrderStatusService : IUpdateOrderStatusService
    {
        private readonly IOrderQuery _orderQuery;
        public UpdateOrderStatusService(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }
        public async Task UpdateOrderStatusBasedOnItemsAsync(Order order)
        {
            int newStatus;
            if (order == null || order.OrderItems == null || order.OrderItems.Count == 0)
                return; // No hay orden o items para evaluar

            var activeItems = order.OrderItems.Where(i => i.Status != 5).ToList(); // Solo items activos (no cancelados)
            
            if (activeItems.Count == 0)
            {
                newStatus = 5; // Todos los items cancelados = Orden cancelada
            }
            else
            {
                newStatus = activeItems.Min(i => i.Status); // Tomar el menor estado entre los ítems activos
            }

            order.OverallStatus = newStatus;
            await _orderQuery.OrderUpdateAsync(order);
        }
    }
}
