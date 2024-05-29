using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Login;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/logging/")]
    public class LoggingController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        //public LoggingController(IPublishEndpoint publishEndpoint)
        //{
        //    _publishEndpoint = publishEndpoint;
        //}
        private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet("check")]
        public async Task<IActionResult> checkLogin(String login)
        {
            try
            {
                var clientResponse = await _requestClient.GetResponse<CheckLoginEventReply>(
                    new CheckLoginEvent() { Login = login });
                if (clientResponse.Message.LoggedIn == CheckLoginEventReply.State.LOGGED)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
