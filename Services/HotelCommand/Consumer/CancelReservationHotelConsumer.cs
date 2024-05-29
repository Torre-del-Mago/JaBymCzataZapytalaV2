using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class CancelReservationHotelConsumer(IEventService eventService) : IConsumer<CancelReservationHotelEvent>
    {
        public async Task Consume(ConsumeContext<CancelReservationHotelEvent> context)
        {
            await eventService.cancelHotel(context.Message.OfferId);

            await context.RespondAsync(new CancelReservationHotelSyncEvent()
            {
                OfferId = context.Message.OfferId
            });
        }
    }
}
