using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class RemoveWatcherConsumer : IConsumer<RemoveWatcherEvent>
    {
        private readonly IHotelService _service;
        public RemoveWatcherConsumer(IHotelService service)
        {
            _service = service;
        }
        public Task Consume(ConsumeContext<RemoveWatcherEvent> context)
        {
            Console.WriteLine("Hotel received event RemoveWatcherEvent with hotelId: " + context.Message.HotelId);
            _service.RemoveWatcher(context.Message.HotelId);
            return Task.CompletedTask;
        }
    }
}