using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Login;
using Models.Offer;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/logging/")]
    public class LoggingController : ControllerBase
    {
        private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet("check")]
        public async Task<IActionResult> checkLogin(String login)
        {
            try {
                var clientResponse = await _requestClient.GetResponse<CheckLoginEventReply> (
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
