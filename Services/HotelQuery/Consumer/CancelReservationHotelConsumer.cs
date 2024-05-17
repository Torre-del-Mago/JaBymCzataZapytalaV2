using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class CancelReservationHotelConsumer : IConsumer<CancelReservationHotelEvent>
    {
        private readonly IHotelService _service;
        public CancelReservationHotelConsumer(IHotelService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CancelReservationHotelEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
