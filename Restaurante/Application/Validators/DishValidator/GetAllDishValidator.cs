using Application.Enum;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using static Application.Validators.Exceptions;

namespace Application.Validators.DishValidator
{
    public class GetAllDishValidator : IGetAllDishValidation
    {
        private readonly ICategoryQuery _categoryQuery;

        public GetAllDishValidator(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }
        public async Task ValidateAllAsync(string? name, int? category, OrderPrice? sortByPrice)
        {
            // Validación de nombre
            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalized = name.Trim().ToLowerInvariant();

                if (normalized.Length > 100)
                    throw new BadRequestException("El nombre no puede superar los 100 caracteres");
            }
            // Validación de categoría
            if (category.HasValue)
            {
                if (category.Value <= 0)
                    throw new BadRequestException("La categoría debe ser mayor a cero");

                var exists = await _categoryQuery.GetByCategoryIdAsync(category.Value);
                if (exists == null)
                    throw new NotFoundException("La categoría no existe");
            }
            // Validación de orden de precio
            if (sortByPrice.HasValue && sortByPrice is not (OrderPrice.asc or OrderPrice.desc))
            {
                throw new BadRequestException("Orden de precio inválido");
            }
        }
    }
}
