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
            if (order.Items == null || !order.Items.Any())
                throw new BadRequestException("La orden debe contener al menos un ítem.");

            if (order.Delivery == null || string.IsNullOrWhiteSpace(order.Delivery.To))
                throw new BadRequestException("Debe especificarse la dirección de entrega.");

            if (order.Delivery != null && order.Delivery.Id <= 0)
                throw new BadRequestException("El identificador de entrega debe ser mayor que 0.");

            foreach (var item in order.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException("La cantidad debe ser mayor que 0.");

                var dish = await _dishQuery.GetDishByIdAsync(item.Id);
                if (dish == null || !dish.Available)
                    throw new BadRequestException("El plato especificado no existe o no está disponible.");
            }
        }
    }
}
