using Login.Service.LoginService;
using MassTransit;
using Models.Login;

namespace Login.Consumer
{
    public sealed class LoginConsumer : IConsumer<CheckLoginEvent>
    {
        //private ILogger _logger { get; set; }
        private ILoginService _service { get; set; }

        //public LoginConsumer(ILoginService loginService)
        //{
        //    _service = loginService;
        //}

        public async Task Consume(ConsumeContext<CheckLoginEvent> context)
        {
            Console.WriteLine("AAA");
            //_logger.LogInformation("AAA");
            //var userLoggedIn = _service.isUsernameCorrect(context.Message.Login);

            //await context.Publish(new CheckLoginEventReply() { LoggedIn = userLoggedIn ? 
            //    CheckLoginEventReply.State.LOGGED : 
            //    CheckLoginEventReply.State.UNLOGGED });
        }
    }
}
