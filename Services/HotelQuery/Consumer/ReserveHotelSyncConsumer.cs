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
            throw new NotImplementedException();
        }
    }
}
