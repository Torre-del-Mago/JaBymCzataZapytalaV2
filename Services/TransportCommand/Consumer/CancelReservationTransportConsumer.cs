using MassTransit;
using Models.Transport;
using TransportCommand.Service;

namespace TransportCommand.Consumer
{
    public class CancelReservationTransportConsumer : IConsumer<CancelReservationTransportEvent>
    {
        private IEventService _eventService;
        private IPublishEndpoint _publishEndpoint;
        public CancelReservationTransportConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        {
            _eventService = eventService;
            _publishEndpoint = publishEndpoint; 
        }

        public async Task Consume(ConsumeContext<CancelReservationTransportEvent> context)
        {
            await _eventService.cancelTransport(context.Message.ArrivalTicketId, context.Message.ReturnTicketId);

            await _publishEndpoint.Publish(new CancelReservationTransportSyncEvent()
            {
                ArrivalTicketId = context.Message.ArrivalTicketId,
                ReturnTicketId = context.Message.ReturnTicketId
            });

        }
    }
}
