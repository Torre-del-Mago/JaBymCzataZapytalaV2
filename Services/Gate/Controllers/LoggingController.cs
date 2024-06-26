﻿using MassTransit;
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

        private IRequestClient<CheckLoginEvent> _requestClient { get; set; }

        public LoggingController(IRequestClient<CheckLoginEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet("check")]
        public async Task<IActionResult> checkLogin([FromQuery] String login)
        {
            try
            {
                Console.Out.WriteLine("Got Request checkLogin ");
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

        [HttpGet("test-check")]
        public async Task<IActionResult> testCheckLogin([FromQuery] String login)
        {
            try
            {
                bool isLoggedIn = login == "zbysio" ||
                     login == "kasia" ||
                     login == "henio";
                if (isLoggedIn)
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
