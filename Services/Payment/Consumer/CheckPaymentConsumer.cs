using MassTransit;
using Models.Payment;

namespace Payment.Consumer
{
    public class CheckPaymentConsumer : IConsumer<CheckPaymentEvent>
    {
        public async Task Consume(ConsumeContext<CheckPaymentEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new CheckPaymentEventReply() { });
        }
    }
}
