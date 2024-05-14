using MassTransit;
using Models.Login;

namespace Login.Consumer
{
    public class LoginConsumer : IConsumer<CheckLoginEvent>
    {
        public async Task Consume(ConsumeContext<CheckLoginEvent> context)
        {
            /*
             Do something
             */
            await context.Publish(new CheckLoginEventReply() { });
        }
    }
}
