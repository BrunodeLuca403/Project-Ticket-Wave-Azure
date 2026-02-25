using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.application.Dto;
using TicketAzure.application.Services.Interface;
using TicketAzure.Core.Dto;
using TicketAzure.Core.Entitys;
using TicketAzure.Core.Enum;
using TicketAzure.Core.Repositories;
using TicketAzure.Core.Services;

namespace TicketAzure.application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPaymentRepository _paymentRepository; 
        private readonly IPaymentExternalService _paymentExternalService;
        private readonly IMenssagePublish _menssagePublish;
        public PaymentService(IEventRepository eventRepository, IPaymentRepository paymentRepository, IPaymentExternalService paymentExternalService, IMenssagePublish menssagePublish)
        {
            _eventRepository = eventRepository;
            _paymentRepository = paymentRepository;
            _paymentExternalService = paymentExternalService;
            _menssagePublish = menssagePublish;
        }

        public async Task<Result<PaymentResultDto>> CreatePayment(PaymentCreateDto payment, CancellationToken cancellationToken)
        {
            var @event = _eventRepository.GetByIdAsync(payment.EventId, CancellationToken.None).Result;

            if (@event is null)
            {
                return Result<PaymentResultDto>.Fail(
                    new NotFoundError($"Event with id {payment.EventId} not found.")
                );
            }

            var paymentExternal = await _paymentExternalService.GeneratePixAsync(@event.Price * payment.QuantityTicket);

            if(paymentExternal is null || string.IsNullOrEmpty(paymentExternal.TxId) || string.IsNullOrEmpty(paymentExternal.PixCopyAndPast))
            {
                return Result<PaymentResultDto>.Fail(
                     new InvalidError("Erro ao gerar pagamento PIX.")
                );
            }

            var paymentResult = new Payment
            {
                ExternalId = paymentExternal.TxId,
                EventId = payment.EventId,
                Status = PaymentStatusType.Pending,
                PixCopyAndPaste = paymentExternal.PixCopyAndPast
            };

            await _paymentRepository.CreatePaymentAsync(paymentResult, cancellationToken);

            return Result<PaymentResultDto>.Ok(new PaymentResultDto
            {
               Id =  paymentResult.Id,
               PixCopyAndPast = paymentResult.PixCopyAndPaste,
               Status =  paymentResult.Status,
               CreatedAt = paymentResult.CreatedAt
            });
        }

        public Task<Result<bool>> ProcessPayment(PaymentPaidEvent payment)
        {
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public async Task<Result<bool>> WebHorkFI(WebHookEfi request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Pix == null || !request.Pix.Any())
                {
                    return Result<bool>.Fail(
                         new InvalidError("Lista de pix é obrigatória.")
                     );
                }

                var processedPayments = new List<PaymentResultDto>();
                foreach (var pix in request.Pix)
                {
                    if (string.IsNullOrEmpty(pix.Txid))
                    {
                        return Result<bool>.Fail(
                            new InvalidError("Txid do pix é obrigatório.")
                        );
                    }

                    var payment = await _paymentRepository.GetByExternalIdAsync(pix.Txid, cancellationToken);

                    if (payment is null)
                    {
                        return Result<bool>.Fail(
                            new NotFoundError($"Pagamento com id externo {pix.Txid} não encontrado.")
                        );
                    }

                    payment.Status = PaymentStatusType.Paid;

                    await _paymentRepository.UpdateAsync(payment, cancellationToken);

                    var Json = System.Text.Json.JsonSerializer.Serialize(new PaymentPaidEvent(payment.Id));

                    await _menssagePublish.PublishAsync("payment-confirmed" , Json, cancellationToken);

                }

                return Result<bool>.Ok(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(
                    new InvalidError($"Erro ao processar webhook: {ex.Message}")
                );
            }
        }


        public class PaymentPaidEvent(Guid paymentId);
    }
}
