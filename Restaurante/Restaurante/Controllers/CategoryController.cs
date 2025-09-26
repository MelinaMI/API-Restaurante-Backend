using Application.Interfaces.ICategory;
using Application.Models.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryQuery _categoryQuery;
        public CategoryController(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _categoryQuery.GetAllCategoriesAsync();

            var response = categories
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Order = c.Order
                })
                .ToList();

            return Ok(response);
        }
    }
}
