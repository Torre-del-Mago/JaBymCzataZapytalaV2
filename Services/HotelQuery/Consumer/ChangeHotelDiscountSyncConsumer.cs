using HotelQuery.Service.Hotel;
using MassTransit;
using Models.TravelAgency;

namespace HotelQuery.Consumer;

public class ChangeHotelDiscountSyncConsumer(IHotelService hotelService) : IConsumer<ChangeHotelDiscountSyncEvent>
{
    public Task Consume(ConsumeContext<ChangeHotelDiscountSyncEvent> context)
    {       
        Console.Out.WriteLine("Got event ChangeHotelDiscountSyncEvent for hotel:" + context.Message.HotelId);
        hotelService.ChangeHotelDiscount(context.Message.HotelId, context.Message.DiscountChange);
        return Task.CompletedTask;
    }
}