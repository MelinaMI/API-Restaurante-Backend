using Application.Interfaces.IDish;
using Domain.Entities;
using static Application.Validators.Exceptions;

namespace Application.Validators.DishValidator
{
    public class DeleteDishValidator : IDeleteDishValidation
    {
        public Task DeleteDishValidationAsync(Dish dish)
        {
            if (!dish.Available)
                throw new ConflictException("El plato ya está marcado como no disponible");

            bool tieneOrdenesActivas = dish.OrderItems.Any(oi =>
                oi.OrderNavigation != null &&
                (oi.OrderNavigation.OverallStatus == 1 || oi.OrderNavigation.OverallStatus == 2)// 1: Pending, 2: In progress
            );

            if (tieneOrdenesActivas)
                throw new ConflictException("No se puede eliminar el plato porque está en órdenes activas");

            return Task.CompletedTask;
        }
    }
}
