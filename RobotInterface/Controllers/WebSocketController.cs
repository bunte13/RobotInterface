using Microsoft.AspNetCore.Mvc;
using RobotInterface.Services;
using System;
using System.Threading.Tasks;

namespace RobotInterface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                await _webSocketService.ConnectAsync();
                await _webSocketService.SendMessageAsync(message);
                await _webSocketService.CloseAsync();
                return Ok("Message sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
