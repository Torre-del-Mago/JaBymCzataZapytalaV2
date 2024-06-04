using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class ReserveHotelSyncConsumer : IConsumer<ReserveHotelSyncEvent>
    {
        private readonly IHotelService _service;
        public ReserveHotelSyncConsumer(IHotelService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveHotelSyncEvent> context)
        {
            Console.Out.WriteLine("Hotel Gets Event ReserveHotelSyncEvent");
            return _service.ReserveHotel(context.Message.Reservation);
        }
    }
}
