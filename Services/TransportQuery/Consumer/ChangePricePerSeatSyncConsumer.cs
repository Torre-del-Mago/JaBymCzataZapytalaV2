using MassTransit;
using Models.TravelAgency;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer;

public class ChangePricePerSeatSyncConsumer(ITransportService service) : IConsumer<ChangePricePerSeatSyncEvent>
{
    public Task Consume(ConsumeContext<ChangePricePerSeatSyncEvent> context)
    {
        Console.Out.WriteLine("Got event ChangePricePerSeatSyncEvent for transport:" + context.Message.TransportId);
        service.ChangePricePerSeat(context.Message.TransportId, context.Message.PriceChange);
        return Task.CompletedTask;
    }
}