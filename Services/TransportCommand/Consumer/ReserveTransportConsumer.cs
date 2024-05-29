using MassTransit;
using Models.Transport;
using TransportCommand.Service;

namespace TransportCommand.Consumer
{
    public class ReserveTransportConsumer : IConsumer<ReserveTransportEvent>
    {
        private IEventService _eventService;
        private IPublishEndpoint _publishEndpoint;
        public ReserveTransportConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        {
            _eventService = eventService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<ReserveTransportEvent> context)
        {
            bool hasReservedTransport = await _eventService.reserveTransport(context.Message.Reservation);
            if (!hasReservedTransport) {
                await _publishEndpoint.Publish(new ReserveTransportEventReply()
                {
                    Answer = ReserveTransportEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });            
            }
            await _publishEndpoint.Publish(new ReserveTransportEventReply()
            {
                Answer = ReserveTransportEventReply.State.RESERVED,
                CorrelationId = context.Message.CorrelationId
            });

            await _publishEndpoint.Publish(new ReserveTransportSyncEvent()
            {
                Reservation = context.Message.Reservation
            });

        }
    }
}
