using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class CancelReservationHotelConsumer(IEventService eventService, IPublishEndpoint publishEndpoint) : IConsumer<CancelReservationHotelEvent>
    {
        public async Task Consume(ConsumeContext<CancelReservationHotelEvent> context)
        {
            await eventService.CancelHotel(context.Message.OfferId);

            await publishEndpoint.Publish(new CancelReservationHotelSyncEvent()
            {
                OfferId = context.Message.OfferId
            });
        }
    }
}
