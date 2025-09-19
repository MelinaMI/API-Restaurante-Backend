using Application.Enum;
using Application.Interfaces.IDish;
using Application.Models.Request;
using Application.Models.Response;
using Microsoft.AspNetCore.Mvc;
using static Application.Validators.Exceptions;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ICreateService _createService;
        private readonly IUpdateService _updateService;
        private readonly ICreateValidation _createValidator;
        private readonly IUpdateValidation _updateValidator;
        private readonly IGetAllDishService _getAllService;
        private readonly IGetAllDishValidation _getAllValidator;
        private readonly IGetDishByIdService _getDishByIdService;
        private readonly IGetDishByIdValidation _getDishByIdValidator;
        private readonly IDeleteDish _deleteDishService;
        //private readonly 

        public DishController(ICreateService createService, IUpdateService updateService, ICreateValidation createValidator, IUpdateValidation updateValidator, IGetAllDishService getAllService, IGetAllDishValidation getAllValidator, IGetDishByIdService getDishByIdService, IGetDishByIdValidation getDishByIdValidator, IDeleteDish deleteDish)
        {
            _createService = createService;
            _updateService = updateService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _getAllService = getAllService;
            _getAllValidator = getAllValidator;
            _getDishByIdService = getDishByIdService;
            _getDishByIdValidator = getDishByIdValidator;
            _deleteDishService = deleteDish;
        }
       
        //Create
        [HttpPost]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDishAsync([FromBody] DishRequest request)
        {
            try
            {
                var result = await _createService.CreateDishAsync(request);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiError { Message = $"Ocurrió un error inesperado: {ex.Message}" });
            }
        }

        //Update
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDishAsync(Guid id, [FromBody] DishUpdateRequest request)
        {
            try
            {
                var result = await _updateService.UpdateDishAsync(id, request);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiError { Message = $"Error interno del servidor: {ex.Message}" });
            }
        }
        // Buscar platos con filtros 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<DishResponse>> GetAllAsync([FromQuery] string? name, [FromQuery] int? category, [FromQuery] string? sortByPrice, [FromQuery] bool onlyActive = true)
        {
            try
            {
                var result = await _getAllService.GetAllDishesAsync(name, category, sortByPrice, onlyActive);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
        }
        // Buscar plato por Id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DishResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _getDishByIdService.GetDishByIdAsync(id);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
        }
        // Delete (soft delete)
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDish(Guid id)
        {
            try
            {
                await _deleteDishService.DeleteDishAsync(id);
                return Ok(new { message = "Plato eliminado correctamente (soft delete)." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiError { Message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}

