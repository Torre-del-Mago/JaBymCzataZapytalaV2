using MassTransit;

namespace Login.Consumer
{
    public class LoginConsumer : IConsumer<LoginEvent>
    {
        public async Task Consume(ConsumeContext<LoginEvent> context)
        {
            /*Do something*/
            await context.Publish(new LoginReplyEvent() { });
        }
    }
}
