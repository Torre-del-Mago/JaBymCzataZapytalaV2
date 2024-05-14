using MassTransit;
using Models.Payment;

namespace Payment.Consumer
{
    public class PayConsumer : IConsumer<PayEvent>
    {
        public async Task Consume(ConsumeContext<PayEvent> context)
        {
            /*
             Do something
             */
            await context.Publish(new PayEventReply() { });
        }
    }
}
