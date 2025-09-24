using Application.Interfaces.IOrder;
using static Application.Validators.Exceptions;

namespace Application.Validators.OrderValidator
{
    public class GetAllOrdersValidator : IGetAllOrdersValidation
    {
        private readonly HashSet<int> _validStatuses = new() { 1, 2, 3, 4, 5 };
        public Task ValidateGetAllOrder(DateTime? from, DateTime? to, int? status)
        {
            if (from.HasValue && to.HasValue && from > to)
                throw new BadRequestException("La fecha de inicio no puede ser posterior a la fecha de fin");

            if (from.HasValue && from.Value > DateTime.UtcNow)
                throw new BadRequestException("La fecha de inicio no puede estar en el futuro");

            if (to.HasValue && to.Value > DateTime.UtcNow)
                throw new BadRequestException("La fecha de fin no puede estar en el futuro");

            if (status.HasValue && !_validStatuses.Contains(status.Value))
                throw new BadRequestException($"El estado '{status.Value}' no es válido");

            return Task.CompletedTask;
        }
    }
}