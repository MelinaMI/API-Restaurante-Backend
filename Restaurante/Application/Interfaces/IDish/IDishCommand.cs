using Domain.Entities;
// Responsabilidad: Crear y actualizar platos.
namespace Application.Interfaces.IDish
{
    public interface IDishCommand
    {
        Task InsertDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);

    }
}
