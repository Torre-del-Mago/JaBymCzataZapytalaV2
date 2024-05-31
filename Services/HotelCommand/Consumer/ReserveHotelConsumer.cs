using HotelCommand.Repository.ReservationEventRepository;
using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class ReserveHotelConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        : IConsumer<ReserveHotelEvent>
    {
        public async Task Consume(ConsumeContext<ReserveHotelEvent> context)
        {
            Console.WriteLine("Get ReserveHotelEvent");
            var hasReservedHotel= await eventService.ReserveHotel(context.Message.Reservation);
            if (!hasReservedHotel) {
                await context.Publish(new ReserveHotelEventReply()
                {
                    Answer = ReserveHotelEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });
                
            }
            else
            {
                await context.Publish(new ReserveHotelEventReply()
                {
                    Answer = ReserveHotelEventReply.State.RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });
            }
            Console.WriteLine("Send ReserveHotelEventReply");

            await publishEndpoint.Publish(new ReserveHotelSyncEvent()
            {
                Reservation = context.Message.Reservation
            });
            Console.WriteLine("Send ReserveHotelSyncEvent");
        }
    }
}
