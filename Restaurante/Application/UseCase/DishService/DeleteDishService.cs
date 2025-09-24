using Application.Interfaces.IDish;
using static Application.Validators.Exceptions;

namespace Application.UseCase.Services.DishService
{
    public class DeleteDishService : IDeleteDish
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDeleteDishValidation _deleteValidation;
        private readonly IDishCommand _dishCommand;

        public DeleteDishService(IDishQuery dishQuery, IDeleteDishValidation deleteValidation, IDishCommand dishCommand)
        {
            _dishQuery = dishQuery;
            _deleteValidation = deleteValidation;
            _dishCommand = dishCommand;
        }
        public async Task DeleteDishAsync(Guid id)
        {
            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("Plato no encontrado");

            await _deleteValidation.DeleteDishValidationAsync(dish);

            await _dishCommand.DeleteDishAsync(dish); // Acá se aplica isActive = false
        }
    }
}
