using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Models.Request;
using static Application.Validators.Exceptions;

namespace Application.Validators.OrderValidator
{
    public class UpdateOrderValidator : IUpdateOrderValidation
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;
        public UpdateOrderValidator(IOrderQuery orderQuery, IDishQuery dishQuery)
        {
            _orderQuery = orderQuery;
            _dishQuery = dishQuery;
        }

        public async Task UpdateOrderValidation(long id, List<Items> items)
        {
            // Validar existencia de la orden
            var order = await _orderQuery.GetOrderByIdAsync(id);
            if (order == null)
                throw new NotFoundException("La orden no existe");
            // Validar que no esté cerrada
            if (order.OverallStatus == 5)
                throw new BadRequestException("No se puede modificar una orden cerrada");
            // Validar items
            foreach (var item in items)
            {
                var dish = await _dishQuery.GetDishByIdAsync(item.Id);
                if (dish == null)
                    throw new NotFoundException($"El plato con id {item.Id} no existe");

                if (!dish.Available)
                    throw new BadRequestException($"El plato {dish.Name} está inactivo y no se puede agregar");
            }
        
        }
    }
}
