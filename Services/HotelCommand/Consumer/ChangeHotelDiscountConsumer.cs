using HotelCommand.Service;
using MassTransit;
using Models.TravelAgency;

namespace HotelCommand.Consumer;

public class ChangeHotelDiscountConsumer(IEventService eventService, IPublishEndpoint publishEndpoint) : IConsumer<ChangeHotelDiscountEvent>
{
    public async Task Consume(ConsumeContext<ChangeHotelDiscountEvent> context)
    {
        Console.Out.WriteLine("Got event ChangeHotelDiscountEvent for hotel:" + context.Message.HotelId);
        eventService.ChangeHotelDiscount(context.Message.HotelId, context.Message.DiscountChange);
        await publishEndpoint.Publish(new ChangeHotelDiscountSyncEvent()
        {
            HotelId = context.Message.HotelId,
            DiscountChange = context.Message.DiscountChange
        });
    }
}