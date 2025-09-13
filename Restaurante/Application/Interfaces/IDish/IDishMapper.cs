using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDishMapper
    {
        DishResponse ToDishResponse(Dish dish, Category category);
    }
}
