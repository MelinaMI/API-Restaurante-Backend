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
            var order = await _orderQuery.GetOrderByIdAsync(id);

            if (order == null)
                throw new NotFoundException("La orden no existe");

            if (order.OverallStatus == 5)
                throw new BadRequestException("No se puede modificar una orden cerrada");

            /*if (order.OverallStatus == 2)
                throw new BadRequestException("No se puede modificar una orden que ya está en preparación");*/

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException("La cantidad debe ser mayor a cero");
                if (item.Quantity > 50)
                    throw new BadRequestException($"La cantidad no puede superar los 50 por ítem");

                var dish = await _dishQuery.GetDishByIdAsync(item.Id);

                if (dish == null)
                    throw new NotFoundException("El plato especificado no existe");

                if (!dish.Available)
                    throw new BadRequestException("El plato especificado no está disponible");
            }
        }
    }
}
