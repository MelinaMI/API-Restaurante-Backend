using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Response;
using static Application.Validators.Exceptions;

namespace Application.UseCase.Services.DishService
{
    public class DeleteDishService : IDeleteDish
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDeleteDishValidation _deleteValidation;
        private readonly IDishCommand _dishCommand;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IDishMapper _dishMapper;

        public DeleteDishService(IDishQuery dishQuery, IDeleteDishValidation deleteValidation, IDishCommand dishCommand, ICategoryQuery categoryQuery, IDishMapper dishMapper)
        {
            _dishQuery = dishQuery;
            _deleteValidation = deleteValidation;
            _dishCommand = dishCommand;
            _categoryQuery = categoryQuery;
            _dishMapper = dishMapper;
        }
        public async Task<DishResponse> DeleteDishAsync(Guid id)
        {
            await _deleteValidation.DeleteDishValidation(id);

            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("Plato no encontrado");
            await _dishCommand.DeleteDishAsync(id);
            var category = await _categoryQuery.GetByCategoryIdAsync(dish.Category); 
            return _dishMapper.ToDishResponseList(dish, category);
        }
    }
}
