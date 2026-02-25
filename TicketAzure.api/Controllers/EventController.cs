using Microsoft.AspNetCore.Mvc;
using TicketAzure.application.Dto;
using TicketAzure.application.Services.Interface;

namespace TicketAzure.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {

        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _eventService.ExecuteAsync(request, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("tickets-all")]

        public async Task<IActionResult> GetAllEvents(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _eventService.GetAllAsync(cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
