using Domain.Entities;
//Responsabilidad: Obtener información de platos, con filtros y ordenamientos.
namespace Application.Interfaces.IDish
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAllAsync();
        Task<Dish> GetByNameAsync(string name);
        Task<Dish> GetDishByIdAsync(Guid id);
        Task<decimal> GetDishPriceAsync(Guid id);
    }
}
