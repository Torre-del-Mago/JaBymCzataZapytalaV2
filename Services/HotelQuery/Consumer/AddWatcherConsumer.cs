using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class AddWatcherConsumer : IConsumer<AddWatcherEvent>
    {
        private readonly IHotelService _service;
        public AddWatcherConsumer(IHotelService service)
        {
            _service = service;
        }
        public Task Consume(ConsumeContext<AddWatcherEvent> context)
        {
            Console.WriteLine("Hotel received event AddWatcherEvent with hotelId: " + context.Message.HotelId);
            _service.AddWatcher(context.Message.HotelId);
            return Task.CompletedTask;
        }
    }
}