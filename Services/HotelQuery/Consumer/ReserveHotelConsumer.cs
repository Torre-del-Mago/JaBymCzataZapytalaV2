using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelEvent>
    {
        private readonly IHotelService _service;
        public ReserveHotelConsumer(IHotelService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveHotelEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
