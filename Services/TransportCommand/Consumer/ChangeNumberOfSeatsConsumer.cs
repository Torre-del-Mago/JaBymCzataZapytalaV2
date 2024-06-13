using MassTransit;
using Models.TravelAgency;
using TransportCommand.Service;

namespace TransportCommand.Consumer;

public class ChangeNumberOfSeatsConsumer(IEventService eventService, IPublishEndpoint publishEndpoint) : IConsumer<ChangeNumberOfSeatsEvent>
{
    public async Task Consume(ConsumeContext<ChangeNumberOfSeatsEvent> context)
    {
        Console.Out.WriteLine("Got event ChangeNumberOfSeatsEvent for transport:" + context.Message.TransportId);
        int actualNumberOfSeatsUpdated = eventService.ChangeNumberOfSeats(context.Message.TransportId, context.Message.NumberOfSeats);
        await publishEndpoint.Publish(new ChangeNumberOfSeatsSyncEvent()
        {
            TransportId = context.Message.TransportId,
            NumberOfSeats = actualNumberOfSeatsUpdated
        });
    }
}