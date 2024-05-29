using Login.Service.LoginService;
using MassTransit;
using Models.Login;

namespace Login.Consumer
{
    public sealed class LoginConsumer : IConsumer<CheckLoginEvent>
    {
        private ILoginService _service { get; set; }

        //public LoginConsumer(ILoginService loginService)
        //{
        //    _service = loginService;
        //}

        public async Task Consume(ConsumeContext<CheckLoginEvent> context)
        {
            var @event = context.Message;
            var userLoggedIn = _service.isUsernameCorrect(context.Message.Login);
            await context.RespondAsync(new CheckLoginEventReply()
            {
                CorrelationId = @event.CorrelationId,
                LoggedIn = userLoggedIn ?
                CheckLoginEventReply.State.LOGGED :
                CheckLoginEventReply.State.UNLOGGED
            });
        }
    }
}
