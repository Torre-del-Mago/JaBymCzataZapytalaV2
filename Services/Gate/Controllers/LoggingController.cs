using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Login;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/logging/")]
    public class LoggingController
    {
        private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet("check")]
        public async IEnumerable<> checkLogin(String login)
        {
            var request = new CheckLoginEvent()
            {
            };
            var response = await _requestClient.GetResponse<CheckLoginEventReply>(request);
            return response.Message.Status;
        }
    }
}
