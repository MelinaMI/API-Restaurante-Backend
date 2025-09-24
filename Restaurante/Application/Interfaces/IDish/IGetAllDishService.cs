using Application.Models.Response;
using Application.Enum;

//Responsabilidad: Coordinar validaciones, transformar DTOs, orquestar comandos y consultas.
namespace Application.Interfaces.IDish
{
    public interface IGetAllDishService
    {
        Task<IReadOnlyList<DishResponse>> GetAllDishesAsync(string? name, int? category, OrderPrice? sortByPrice, bool onlyActive);
    }
}
