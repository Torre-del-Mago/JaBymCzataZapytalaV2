using MassTransit;
using Models.TravelAgency;
using TransportCommand.Service;

namespace TransportCommand.Consumer;

public class ChangePricePerSeatConsumer(IEventService eventService, IPublishEndpoint publishEndpoint) : IConsumer<ChangePricePerSeatEvent>
{
    public async Task Consume(ConsumeContext<ChangePricePerSeatEvent> context)
    {
        Console.Out.WriteLine("Got event ChangePricePerSeatEvent for transport:" + context.Message.TransportId);
        eventService.ChangePricePerSeat(context.Message.TransportId, context.Message.PriceChange);
        await publishEndpoint.Publish(new ChangePricePerSeatSyncEvent()
        {
            TransportId = context.Message.TransportId,
            PriceChange = context.Message.PriceChange
        });
    }
}