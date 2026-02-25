using Microsoft.AspNetCore.Mvc;
using TicketAzure.application.Dto;
using TicketAzure.application.Services.Interface;

namespace TicketAzure.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto payment, CancellationToken cancellationToken)
        {
            var result = await _paymentService.CreatePayment(payment, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors);
        }


        [HttpPost("web-hook-payment")]
        public async Task<IActionResult> WebHookPayment([FromBody] PaymentCreateDto payment, CancellationToken cancellationToken)
        {
            var result = await _paymentService.CreatePayment(payment, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors);
        }


    }
}
