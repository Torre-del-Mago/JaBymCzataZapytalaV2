using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class GetHotelStatisticsConsumer :IConsumer<GetHotelStatisticsEvent>
    {
        public GetHotelStatisticsConsumer()
        {
            
        }
        public async Task Consume(ConsumeContext<GetHotelStatisticsEvent> context)
        {
            Console.WriteLine("Hotel received event GetHotelStatisticsEvent with hotelId: " + context.Message.HotelId);
            await context.RespondAsync(new GetHotelStatisticsEventReply
            {
                CorrelationId = context.Message.CorrelationId,
                HasSomebodyReservedHotel = false,
                IsSomeoneElseWatching = false
            });
        }
    }
}