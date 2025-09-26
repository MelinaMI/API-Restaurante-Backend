using Application.Interfaces.IDeliveryType;
using Application.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        private readonly IDeliveryTypeMapper _mapper;
        public DeliveryTypeController(IDeliveryTypeQuery deliveryTypeQuery, IDeliveryTypeMapper mapper)
        {
            _deliveryTypeQuery = deliveryTypeQuery;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<GenericResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<GenericResponse>>> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _deliveryTypeQuery.GetAllDeliveryTypesAsync();
            var response = deliveryTypes.Select(_mapper.ToGenericResponse).ToList();
            return Ok(response);
        }
    }
}
