using Application.Interfaces.IDish;
using static Application.Validators.Exceptions;

namespace Application.Validators.DishValidator
{
    public class GetDishByIdValidator : IGetDishByIdValidation
    {
        private readonly IDishQuery _dishQuery;
        public GetDishByIdValidator(IDishQuery query)
        {
            _dishQuery = query;
        }
        public async Task ValidateByIdAsync(Guid id)
        {
            var dish = await _dishQuery.GetDishByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("Plato no encontrado");
        }
    }
}
