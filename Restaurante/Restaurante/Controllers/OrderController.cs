using Application.Interfaces.IOrder;
using Application.Models.Request;
using Application.Models.Response;
using Application.Services.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Application.Validators.Exceptions;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetAllOrders _orderService;

        public OrderController(ICreateOrderService createOrderService, IGetAllOrders orderService)
        {
            _createOrderService = createOrderService;
            _orderService = orderService;
        }

        [HttpPost]

        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderCreateResponse>> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                var response = await _createOrderService.CreateOrderAsync(request);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsResponse>>> GetAllOrders([FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] int? status)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(from, to, status);

                if (!orders.Any())
                    return NotFound("No se encontraron órdenes con los filtros aplicados.");

                return Ok(orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al obtener las órdenes.", detail = ex.Message });
            }
        }

    }
}
