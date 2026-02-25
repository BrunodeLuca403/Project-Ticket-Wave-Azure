using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.application.Dto;
using TicketAzure.Core.Dto;
using static TicketAzure.application.Services.PaymentService;

namespace TicketAzure.application.Services.Interface
{
    public interface IPaymentService
    {
        Task<Result<PaymentResultDto>> CreatePayment(PaymentCreateDto payment, CancellationToken cancellationToken);
        Task<Result<bool>> WebHorkFI(WebHookEfi payment, CancellationToken cancellationToken);

        Task<Result<bool>> ProcessPayment(PaymentPaidEvent payment);


    }
}
