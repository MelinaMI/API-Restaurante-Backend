using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using System.Text;
using static Application.Validators.Exceptions;

namespace Application.Validators.DishValidator
{
    public class CreateDishValidator : ICreateValidation
    {
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;
        public CreateDishValidator(IDishQuery query, ICategoryQuery categoryQuery)
        {
            _dishQuery = query;
            _categoryQuery = categoryQuery;
        }
        public async Task ValidateCreateAsync(DishRequest request)
        {
            var nameNormalized = request.Name?.Trim().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(nameNormalized))
                throw new BadRequestException("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new BadRequestException("El precio debe ser mayor a cero");

            if (request.Category <= 0)
                throw new BadRequestException("La categoría es obligatoria");

            var category = await _categoryQuery.GetByCategoryIdAsync(request.Category);
            if (category == null)
                throw new NotFoundException("La categoría no existe");

            var existingDish = await _dishQuery.GetByNameAsync(nameNormalized);
            if (existingDish != null)
                throw new ConflictException("Ya existe un plato con ese nombre");
        }
    }
}
