using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class RemoveWatcherConsumer : IConsumer<RemoveWatcherEvent>
    {
        public RemoveWatcherConsumer()
        {
            
        }
        public Task Consume(ConsumeContext<RemoveWatcherEvent> context)
        {
            Console.WriteLine("Hotel received event RemoveWatcherEvent with hotelId: " + context.Message.HotelId);
            return Task.CompletedTask;
        }
    }
}