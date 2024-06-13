using HotelQuery.Service.Hotel;
using MassTransit;
using Models.TravelAgency;

namespace HotelQuery.Consumer;

public class RegisterTransportAgencyChangeConsumer(IHotelService hotelService) : IConsumer<RegisterTransportAgencyChangeEvent>
{
    public Task Consume(ConsumeContext<RegisterTransportAgencyChangeEvent> context)
    {
        Console.WriteLine("Hotel received event RegisterTransportAgencyChangeEvent with hotelId:" + context.Message.IdChanged + " and value: " + context.Message.Change);
        hotelService.RegisterTransportAgencyChange(context.Message.EventName, context.Message.IdChanged, context.Message.Change);
        return Task.CompletedTask;
    }
}