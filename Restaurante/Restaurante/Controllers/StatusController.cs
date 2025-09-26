using Application.Interfaces.IStatus;
using Application.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusQuery _statusQuery;
        private readonly IStatusMapper _mapper;
        public StatusController(IStatusQuery statusQuery, IStatusMapper mapper)
        {
            _statusQuery = statusQuery;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<GenericResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<GenericResponse>>> GetAllDeliveryTypes()
        {
            var status = await _statusQuery.GetAllStatusesAsync();
            var response = status.Select(_mapper.ToGenericResponse).ToList();
            return Ok(response);
        }
    }
}
