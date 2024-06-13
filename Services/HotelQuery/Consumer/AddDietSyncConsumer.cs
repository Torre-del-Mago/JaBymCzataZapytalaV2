using HotelQuery.Service.Hotel;
using MassTransit;
using Models.TravelAgency;

namespace HotelQuery.Consumer;

public class AddDietSyncConsumer(IHotelService hotelService) : IConsumer<AddDietSyncEvent>
{
    public Task Consume(ConsumeContext<AddDietSyncEvent> context)
    {
        Console.Out.WriteLine("Got event AddDietSyncEvent for hotel:" + context.Message.HotelId);
        hotelService.AddDiet(context.Message.HotelId, context.Message.DietId);
        return Task.CompletedTask;
    }
}