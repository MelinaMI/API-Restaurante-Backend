using Application.Enum;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ICreateService _createService;
        private readonly IUpdateService _updateService;
        private readonly IGetAllDishService _getAllService;
        private readonly IGetDishByIdService _getDishByIdService;
        private readonly IDeleteDish _deleteDishService;
        public DishController(ICreateService createService, IUpdateService updateService, IGetAllDishService getAllService, IGetDishByIdService getDishByIdService, IDeleteDish deleteDish)
        {
            _createService = createService;
            _updateService = updateService;
            _getAllService = getAllService;
            _getDishByIdService = getDishByIdService;
            _deleteDishService = deleteDish;
        }
        //Create
        [HttpPost]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDishAsync([FromBody] DishRequest request)
        {
            var result = await _createService.CreateDishAsync(request);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        //Update
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDishAsync(Guid id, [FromBody] DishUpdateRequest request)
        {
            var result = await _updateService.UpdateDishAsync(id, request);
            return Ok(result);
        }
        // Buscar platos con filtros 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<DishResponse>> GetAllAsync([FromQuery] string? name, [FromQuery] int? category, [FromQuery] OrderPrice? sortByPrice, [FromQuery] bool onlyActive = true)
        {
            var result = await _getAllService.GetAllDishesAsync(name, category, sortByPrice, onlyActive);
            return Ok(result);
        }
        // Buscar plato por Id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<DishResponse>> GetByIdAsync(Guid id)
        {
            var result = await _getDishByIdService.GetDishByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DishResponse>> DeleteDish([FromRoute] Guid id)
        {
            var deletedDish = await _deleteDishService.DeleteDishAsync(id);
            return NoContent();
        }
    }
}

