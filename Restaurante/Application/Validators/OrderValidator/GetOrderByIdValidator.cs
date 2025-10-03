using Application.Interfaces.IOrder;
using static Application.Validators.Exceptions;

namespace Application.Validators.OrderValidator
{
    public class GetOrderByIdValidator : IGetOrderByIdValidation
    {
        private readonly IOrderQuery _orderQuery;
        public GetOrderByIdValidator(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }   
        public async Task ValidateOrderById(long orderId)
        {
            if (orderId <= 0)
                throw new BadRequestException("El ID de la orden debe ser un número positivo.");

            var order = await _orderQuery.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Orden no encontrada.");
        }
    }
}
