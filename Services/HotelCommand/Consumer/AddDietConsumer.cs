using HotelCommand.Service;
using MassTransit;
using Models.TravelAgency;

namespace HotelCommand.Consumer;

public class AddDietConsumer(IEventService eventService, IPublishEndpoint publishEndpoint) : IConsumer<AddDietEvent>
{
    public async Task Consume(ConsumeContext<AddDietEvent> context)
    {
        Console.Out.WriteLine("Got event AddDietEvent for hotel:" + context.Message.HotelId);
        var addedDiet = await eventService.AddDiet(context.Message.HotelId, context.Message.DietId);
        await publishEndpoint.Publish(new AddDietSyncEvent()
        {
            HotelId = context.Message.HotelId,
            DietId = context.Message.DietId,
            Done = addedDiet
        });
    }
}