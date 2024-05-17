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

        //private ILogger _logger { get; set; }

        public LoggingController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        //private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        //public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        //{
        //    _requestClient = requestClient;
        //}

        [HttpGet("check")]
        public async Task<IActionResult> checkLogin(String login)
        {
            //var cts = new CancellationTokenSource();
            //cts.CancelAfter(TimeSpan.FromSeconds(10));
            //var cancellationToken = cts.Token;
            Console.WriteLine("BBB");
            await _publishEndpoint.Publish(new CheckLoginEvent() { Login = login });
            //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/custom-queue-name"));
            Console.WriteLine("CCC");
            //await endpoint.Send(new CheckLoginEvent() { Login = login });
            Console.WriteLine("DDD");
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
