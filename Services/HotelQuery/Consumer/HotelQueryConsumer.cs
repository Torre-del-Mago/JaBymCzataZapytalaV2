using MassTransit;

namespace HotelQuery.Consumer
{
    public class HotelQueryConsumer : IConsumer<HotelQueryEvent>
    {
        public async Task Consume(ConsumeContext<HotelQueryEvent> context)
        {
            /**/
            await context.Publish(new HotelQueryResponseEvent() { })
        }
    }
}
