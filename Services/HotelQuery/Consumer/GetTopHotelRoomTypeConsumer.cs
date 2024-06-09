using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Admin.DTO;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class GetTopHotelRoomTypeConsumer : IConsumer<GetTopHotelRoomTypeEvent>
    {
        private readonly IHotelService _service;

        public GetTopHotelRoomTypeConsumer(IHotelService service)
        {
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<GetTopHotelRoomTypeEvent> context)
        {
            Console.Out.WriteLine("Hotel Gets Event GetTopHotelRoomTypeEvent");
            var @event = context.Message;
            var topHotels = _service.GetTopHotels(10);
            var topRoomTypes = _service.GetTopRoomTypes(10);
            await context.RespondAsync(new GetTopHotelRoomTypeEventReply()
            {
                CorrelationId = @event.CorrelationId,
                TopHotelsDto = topHotels,
                TopRoomTypesDto = topRoomTypes
            });
        }
    }
}