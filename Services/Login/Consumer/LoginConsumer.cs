using Login.Service.LoginService;
using MassTransit;
using Models.Login;

namespace Login.Consumer
{
    public class LoginConsumer : IConsumer<CheckLoginEvent>
    {
        private ILoginService _service { get; set; }

        public LoginConsumer(ILoginService loginService)
        {
            _service = loginService;
        }

        public async Task Consume(ConsumeContext<CheckLoginEvent> context)
        {
            Console.WriteLine("Get CheckLoginEvent");
            var @event = context.Message;
            var userLoggedIn = _service.IsUsernameCorrect(context.Message.Login);
            await context.RespondAsync(new CheckLoginEventReply()
            {
                CorrelationId = @event.CorrelationId,
                LoggedIn = userLoggedIn ?
                CheckLoginEventReply.State.LOGGED :
                CheckLoginEventReply.State.UNLOGGED
            });
            Console.WriteLine("Send CheckLoginEventReply");
        }
    }
}
