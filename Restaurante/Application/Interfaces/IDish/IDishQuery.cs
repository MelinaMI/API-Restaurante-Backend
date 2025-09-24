using Domain.Entities;
//Responsabilidad: Obtener información de platos, con filtros y ordenamientos.
namespace Application.Interfaces.IDish
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAll();
        Task<Dish?> GetByNameAsync(string name);
        Task<Dish?> GetDishByIdAsync(Guid id);
    }
}
