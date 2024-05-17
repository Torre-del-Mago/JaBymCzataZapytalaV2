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
        private readonly ISendEndpointProvider _sendEndpointProvider;

        private ILogger _logger { get; set; }

        public LoggingController(ISendEndpointProvider sendEndpointProvider, ILogger logger)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logger = logger;
        }
        //private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        //public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        //{
        //    _requestClient = requestClient;
        //}

        [HttpGet("check")]
        public async Task<IActionResult> checkLogin(String login)
        {
            _logger.LogInformation("BBB");
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/custom-queue-name"));
            _logger.LogInformation("CCC");
            await endpoint.Send(new CheckLoginEvent() { Login = login });
            _logger.LogInformation("DDD");
            return Ok();
            //try
            //{
            //    var clientResponse = await _requestClient.GetResponse<CheckLoginEventReply>(
            //        new CheckLoginEvent() { Login = login });
            //    if (clientResponse.Message.LoggedIn == CheckLoginEventReply.State.LOGGED)
            //    {
            //        return Ok();
            //    }
            //    else
            //    {
            //        return BadRequest();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return NotFound();
            //}
        }
    }
}
