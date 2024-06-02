using MassTransit;
using Models.Transport;
using TransportCommand.Repository.ReservedTicketRepository;
using TransportCommand.Service;

namespace TransportCommand.Consumer
{
    public class ReserveTransportConsumer : IConsumer<ReserveTransportEvent>
    {
        private IEventService _eventService;
        private IReservedTicketRepository _reservedTicketRepository;
        public ReserveTransportConsumer(IEventService eventService, IReservedTicketRepository reservedTicketRepository)
        {
            _eventService = eventService;
            _reservedTicketRepository = reservedTicketRepository;
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
                var tickets =
                    await _reservedTicketRepository.GetReservedTicketsByOfferId(context.Message.Reservation.OfferId);
                var arrivalTicketId = tickets.First(t => t.TransportId == context.Message.Reservation.ArrivalTransportId).Id;
                var returnTicketId = tickets.First(t => t.TransportId == context.Message.Reservation.ReturnTransportId).Id;
                await context.Publish(new ReserveTransportSyncEvent()
                {
                    Reservation = context.Message.Reservation,
                    ArrivalTicketId = arrivalTicketId,
                    ReturnTicketId = returnTicketId
                });
            }
        }
    }
}
