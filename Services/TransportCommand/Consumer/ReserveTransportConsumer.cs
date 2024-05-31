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
            Console.Out.WriteLine($"Started reserving transport for offer with id {context.Message.Reservation.OfferId}");
            bool hasReservedTransport = await _eventService.ReserveTransport(context.Message.Reservation);
            if (!hasReservedTransport) {
                Console.Out.WriteLine($"Could not reserve transport for offer with id {context.Message.Reservation.OfferId}");
                await context.Publish(new ReserveTransportEventReply()
                {
                    Answer = ReserveTransportEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });
            }
            else
            {
                Console.Out.WriteLine($"Reserved transport for offer with id {context.Message.Reservation.OfferId}");
                await context.Publish(new ReserveTransportEventReply()
                {
                    Answer = ReserveTransportEventReply.State.RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });
            }

            await context.Publish(new ReserveTransportSyncEvent()
            {
                Reservation = context.Message.Reservation
            });

        }
    }
}
