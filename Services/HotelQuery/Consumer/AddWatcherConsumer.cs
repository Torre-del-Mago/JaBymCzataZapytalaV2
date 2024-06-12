using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class AddWatcherConsumer : IConsumer<AddWatcherEvent>
    {
        public AddWatcherConsumer()
        {
            
        }
        public Task Consume(ConsumeContext<AddWatcherEvent> context)
        {
            Console.WriteLine("Hotel received event AddWatcherEvent with hotelId: " + context.Message.HotelId);
            return Task.CompletedTask;
        }
    }
}