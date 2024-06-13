using HotelQuery.Service.Hotel;
using MassTransit;
using Models.TravelAgency;

namespace HotelQuery.Consumer;

public class GetLastTravelAgencyChangesConsumer(IHotelService service) : IConsumer<GetLastTravelAgencyChangesEvent>
{
    public async Task Consume(ConsumeContext<GetLastTravelAgencyChangesEvent> context)
    {
        Console.WriteLine("Hotel received event GetLastTravelAgencyChangesEvent");
        await context.RespondAsync(new GetLastTravelAgencyChangesEventReply
        {
            CorrelationId = context.Message.CorrelationId,
            LastTravelAgencyChangesDto = null
        });
    }
}