using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Models.Request;
using static Application.Validators.Exceptions;

namespace Application.Validators.OrderValidator
{
    public class CreateOrderValidator : ICreateOrderValidation
    {
        private readonly IDishQuery _dishQuery;
        public CreateOrderValidator(IDishQuery dishQuery)
        {
            _dishQuery = dishQuery;
        }
        public async Task ValidateOrderAsync(OrderRequest order)
        {
            // Validar existencia y contenido de ítems
            if (order.Items == null || order.Items.Count == 0)
                throw new BadRequestException("La orden debe contener al menos un ítem.");

            // Validar que no haya ítems nulos o incompletos
            foreach (var item in order.Items)
            {
                if (item == null)
                    throw new BadRequestException("La orden contiene un ítem nulo.");

                if (item.Id == Guid.Empty)
                    throw new BadRequestException("Cada ítem debe tener un ID de plato válido.");

                if (item.Quantity <= 0)
                    throw new BadRequestException("La cantidad de cada ítem debe ser mayor que 0.");

                var dish = await _dishQuery.GetDishByIdAsync(item.Id);
                if (dish == null || !dish.Available)
                    throw new BadRequestException("El plato especificado no existe o no está disponible.");
            }

            // Validar entrega
            if (order.Delivery == null || string.IsNullOrWhiteSpace(order.Delivery.To))
                throw new BadRequestException("Debe especificarse la dirección de entrega.");

            if (order.Delivery.Id <= 0)
                throw new BadRequestException("Debe especificar un tipo de entrega válido.");
        }
    }
}
