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
                // Contar cuántos items hay por cada estado
                newStatus = activeItems
                    .GroupBy(i => i.Status)
                    .OrderByDescending(g => g.Count())  // mayor cantidad → mayoría
                    .ThenByDescending(g => g.Key)       // desempate por estado más avanzado
                    .First()
                    .Key;
            }
            order.OverallStatus = newStatus; //Actualizo
            await _orderQuery.OrderUpdateAsync(order);
        }
    }
}
