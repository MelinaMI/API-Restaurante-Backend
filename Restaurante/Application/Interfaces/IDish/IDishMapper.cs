using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDishMapper
    {
        DishResponse ToDishResponseList(Dish dish, Category category);
        DishResponse ToDishResponse(Dish dish);
    }
}
