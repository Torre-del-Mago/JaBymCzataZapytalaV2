using MassTransit;
using Models.Transport;
using TransportCommand.Service;

namespace TransportCommand.Consumer
{
    public class ReserveTransportConsumer : IConsumer<ReserveTransportEvent>
    {
        private IEventService _eventService;
        public ReserveTransportConsumer(IEventService eventService)
        {
            _eventService = eventService;
        }
        public async Task Consume(ConsumeContext<ReserveTransportEvent> context)
        {
            bool hasReservedTransport = await _eventService.reserveTransport(context.Message.Reservation);
            if (!hasReservedTransport) {
                await context.RespondAsync(new ReserveTransportEventReply()
                {
                    Answer = ReserveTransportEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });            
            }
            await context.RespondAsync(new ReserveTransportEventReply()
            {
                Answer = ReserveTransportEventReply.State.RESERVED,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish(new ReserveTransportSyncEvent()
            {
                Reservation = context.Message.Reservation
            });

        }
    }
}
