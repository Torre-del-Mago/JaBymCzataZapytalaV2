using HotelCommand.Repository.ReservationEventRepository;
using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelEvent>
    {
        private IEventService _eventService;
        private IPublishEndpoint _publishEndpoint;
        public ReserveHotelConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        {
            _eventService = eventService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<ReserveHotelEvent> context)
        {
            bool hasReservedHotel= await _eventService.reserveHotel(context.Message.Reservation);
            if (!hasReservedHotel) {
                await _publishEndpoint.Publish(new ReserveHotelEventReply()
                {
                    Answer = ReserveHotelEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });            
            }
            await _publishEndpoint.Publish(new ReserveHotelEventReply()
            {
                Answer = ReserveHotelEventReply.State.RESERVED,
                CorrelationId = context.Message.CorrelationId
            });

            await _publishEndpoint.Publish(new ReserveHotelSyncEvent()
            {
                Reservation = context.Message.Reservation
            });

        }
    }
}
