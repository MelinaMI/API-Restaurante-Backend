using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;

namespace Application.Interfaces.IDish
{
    public interface IDishMapper
    {
        DishResponse ToDishResponseList(Dish dish, Category category);
        DishResponse ToDishResponse(Dish dish);
        Dish ToDishUpdate(Dish dish, DishUpdateRequest request);
    }
}
