using MassTransit;
using Models.TravelAgency;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer;

public class ChangeNumberOfSeatsSyncConsumer(ITransportService service) : IConsumer<ChangeNumberOfSeatsSyncEvent>
{
    public Task Consume(ConsumeContext<ChangeNumberOfSeatsSyncEvent> context)
    {
        Console.Out.WriteLine("Got event ChangeNumberOfSeatsSyncEvent for transport:" + context.Message.TransportId);
        service.ChangeNumberOfSeats(context.Message.TransportId, context.Message.NumberOfSeats);
        return Task.CompletedTask;
    }
}