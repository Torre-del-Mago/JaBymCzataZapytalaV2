using HotelCommand.Repository.ReservationEventRepository;
using HotelCommand.Repository.ReservationRepository;
using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class ReserveHotelConsumer(IEventService eventService, IReservationRepository reservationRepository, IPublishEndpoint publishEndpoint)
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

                var reservation =
                    await reservationRepository.GetReservationByOfferIdAsync(context.Message.Reservation.OfferId);
                var reservationToSend = context.Message.Reservation;
                reservationToSend.ReservationId = reservation.Id;
                Console.WriteLine("Send ReserveHotelEventReply");

                await publishEndpoint.Publish(new ReserveHotelSyncEvent()
                {
                    Reservation = reservationToSend
                });
                Console.WriteLine("Send ReserveHotelSyncEvent");
            }
        }
    }
}
