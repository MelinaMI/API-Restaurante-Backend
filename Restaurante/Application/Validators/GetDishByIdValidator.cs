using Application.Interfaces.IDish;
using static Application.Validators.Exceptions;

namespace Application.Validators
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
            if (id == Guid.Empty)
                throw new BadRequestException("Formato de ID inválido");

            var dish = await _dishQuery.GetByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("Plato no encontrado");
        }
    }
}
