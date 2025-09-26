using Application.Interfaces.IOrderItem;
using static Application.Validators.Exceptions;

namespace Application.Validators.OrderItemValidator
{
    public class UpdateOrderItemStatusValidator : IUpdateOrderItemStatusValidation
    {
        private static readonly Dictionary<int, List<int>> ValidTransitions = new()
        {
            { 1, new List<int> {2,5} }, // Pendiente -> En preparación
            { 2, new List<int> {3,5} }, // En preparación -> Listo
            { 3, new List<int> {4,5} } , // Listo -> Entregado
            { 4, new List<int>{5} },       // Entregado -> ninguno
            { 5, new List<int>() }       // Cancelada / Cerrada -> ninguno
        };

        private static readonly Dictionary<int, string> StatusNames = new()
        {
            { 1, "Pendiente" },
            { 2, "En preparación" },
            { 3, "Listo" },
            { 4, "Entregado" },
            { 5, "Cancelada"  }
        };

        public Task ValidateUpdateOrderItemStatusAsync(int currentStatus, int newStatus)
        {
            if (newStatus == null)
                throw new BadRequestException("Nuevo estado no puede ser nulo"); 

            if (!StatusNames.ContainsKey(newStatus))
                throw new BadRequestException("El estado especificado no es válido");

            // Validar transición
            if (!ValidTransitions[currentStatus].Contains(newStatus))
            {
                var currentName = StatusNames[currentStatus];
                var newName = StatusNames[newStatus];

                throw new BadRequestException($"No se puede cambiar de '{currentName}' a '{newName}'");
            }

            return Task.CompletedTask;
        }
    }  
}
