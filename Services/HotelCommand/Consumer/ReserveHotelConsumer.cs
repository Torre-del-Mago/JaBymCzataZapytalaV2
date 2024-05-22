using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelEvent>
    {

        public Task Consume(ConsumeContext<ReserveHotelEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
