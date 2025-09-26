using Application.Interfaces.IDish;
using static Application.Validators.Exceptions;

namespace Application.Validators.DishValidator
{
    public class DeleteDishValidator : IDeleteDishValidation
    {
        private readonly IDishQuery _dishQuery;
        public DeleteDishValidator(IDishQuery dishQuery)
        {
            _dishQuery = dishQuery;
        }
        public async Task DeleteDishValidation(Guid id)
        {
            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null) throw new NotFoundException("Plato no encontrado");

            if (!dish.Available)
                throw new ConflictException("El plato ya está marcado como no disponible");

            bool tieneOrdenesActivas = dish.OrderItems.Any(oi =>
                oi.OrderNavigation != null &&
                (oi.OrderNavigation.OverallStatus == 1 || oi.OrderNavigation.OverallStatus == 2) // 1: Pending, 2: In progress
            );

            if (tieneOrdenesActivas)
                throw new ConflictException("No se puede eliminar el plato porque está en órdenes activas");
        }
    }
}
