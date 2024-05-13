using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/logging/")]
    public class LoggingController
    {
        private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        public LoggingController(IRequestClient<CheckLoginEvent> requestClient>)
        {
            _requestClient = requestClient;
        }

        [HttpGet("check")]
        public IEnumerable checkLogin(String login)
        {
            var request = new CheckLoginEvent()
            {
                Login = login
            };
            var response = await _requestClient.GetResponse<CheckLoginReplyEvent>(request);
            return response.Message.Status;
        }
    }
}
