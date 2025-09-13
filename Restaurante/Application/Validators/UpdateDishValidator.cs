using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Models.Request;
using System.Text;
using static Application.Validators.Exceptions;

namespace Application.Validators
{
    public class UpdateDishValidator : IUpdateValidation
    {
        private readonly IDishQuery _dishQuery;
        private readonly ICategoryQuery _categoryQuery;
        public UpdateDishValidator(IDishQuery query, ICategoryQuery categoryQuery)
        {
            _dishQuery = query;
            _categoryQuery = categoryQuery;
        }
        public async Task ValidateUpdateAsync(Guid id, DishUpdateRequest request)
        {
            var nameNormalized = request.Name?.Trim().Normalize(NormalizationForm.FormC).ToLowerInvariant();


            // Validar existencia del plato
            var dish = await _dishQuery.GetByIdAsync(id);
            if (dish == null)
                throw new NotFoundException("El plato no existe");

            if (string.IsNullOrWhiteSpace(nameNormalized))
                throw new BadRequestException("El nombre del plato es obligatorio");

            if (nameNormalized.Length > 100)
                throw new BadRequestException("El nombre no puede superar los 100 caracteres");

            var existing = await _dishQuery.GetByNameAsync(nameNormalized);
            if (existing != null && existing.DishId != id)
                throw new ConflictException("Ya existe otro plato con ese nombre");

            if (request.Price <= 0)
                throw new BadRequestException("El precio debe ser mayor a cero");

            var category = await _categoryQuery.GetByCategoryIdAsync(request.Category);
            if (category == null)
                throw new NotFoundException("La categoría no existe");

        }
    }
}
