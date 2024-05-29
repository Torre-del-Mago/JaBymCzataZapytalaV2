using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class CancelReservationHotelConsumer : IConsumer<CancelReservationHotelEvent>
    {
        private IEventService _eventService;
        private IPublishEndpoint _publishEndpoint;
        public CancelReservationHotelConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        {
            _eventService = eventService;
            _publishEndpoint = publishEndpoint; 
        }

        public async Task Consume(ConsumeContext<CancelReservationHotelEvent> context)
        {
            await _eventService.cancelHotel(context.Message.OfferId);

            await _publishEndpoint.Publish(new CancelReservationHotelSyncEvent()
            {
                OfferId = context.Message.OfferId
            });

        }
    }
}
