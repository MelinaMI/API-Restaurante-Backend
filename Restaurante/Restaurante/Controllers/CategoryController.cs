using Application.Interfaces.ICategory;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryQuery _categoryQuery;
        public CategoryController(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }
        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAllCategories()
        {
            var categories = await _categoryQuery.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
