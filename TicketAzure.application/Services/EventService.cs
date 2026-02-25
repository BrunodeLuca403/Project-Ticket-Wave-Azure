using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.application.Dto;
using TicketAzure.application.Services.Interface;
using TicketAzure.Core.Entitys;
using TicketAzure.Core.Repositories;
using TicketAzure.Core.Services;

namespace TicketAzure.application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStorageService _storageService;
        public EventService(IEventRepository eventRepository, IStorageService storageService)
        {
            _eventRepository = eventRepository;
            _storageService = storageService;
        }

        public async Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken)
        {

            var filePath = await _storageService.UploadFileAsync(request.File.fileName, request.File.base64, cancellationToken.ToString());    

            var @event = new Event(
                request.Name,
                request.Date,
                request.Location,
                request.Description,
                request.Price,
                request.AvailableTickets,
                filePath
            );

            await _eventRepository.CreateAsync(@event, cancellationToken);

            return Result<Guid>.Ok(@event.Id);
        }

        public async Task<Result<List<ListEventDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            var dtos = events.Select(e =>
            {
                var filePath = _storageService.GetSignedUrlAsync(e.ImagePath, TimeSpan.FromMinutes(5), cancellationToken).Result;

                return new ListEventDto
                {
                    id = e.Id,
                    name = e.Name,
                    description = e.Description,
                    price = e.Price,
                    fileName = filePath,
                    createdAt = e.CreatedAt
                };
            }).ToList();

            return Result<List<ListEventDto>>.Ok(dtos);
        }
    }
}
