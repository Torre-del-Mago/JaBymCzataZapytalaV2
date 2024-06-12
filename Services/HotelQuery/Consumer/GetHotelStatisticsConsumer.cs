using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class GetHotelStatisticsConsumer :IConsumer<GetHotelStatisticsEvent>
    {
        private readonly IHotelService _service;
        public GetHotelStatisticsConsumer(IHotelService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<GetHotelStatisticsEvent> context)
        {
            Console.WriteLine("Hotel received event GetHotelStatisticsEvent with hotelId: " + context.Message.HotelId);
            var hasSomebodyReservedHotel = _service.HasSomeoneReservedHotel(context.Message.HotelId);
            var isSomeoneElseWatching = _service.IsSomeoneElseWatching(context.Message.HotelId);
            await context.RespondAsync(new GetHotelStatisticsEventReply
            {
                CorrelationId = context.Message.CorrelationId,
                HasSomebodyReservedHotel = hasSomebodyReservedHotel,
                IsSomeoneElseWatching = isSomeoneElseWatching
            });
        }
    }
}