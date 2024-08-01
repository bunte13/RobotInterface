using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotInterface.Services;
using RobotInterface.ViewModels;
using System.Net.NetworkInformation;

namespace RobotInterface.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RobotController : Controller
    {
        private readonly ILogger<RobotController> _logger;
        private readonly IRobotService _robotService;
        private readonly ISshService _sshService;
        private readonly ISshStateService _sshStateService;

        public RobotController(IRobotService robotService, ISshService sshService, ISshStateService sshStateService, ILogger<RobotController> logger)
        {
            _robotService = robotService;
            _sshService = sshService;
            _sshStateService = sshStateService;
            _logger = logger;
        }

        [HttpGet("set-ip")]
        public IActionResult SetIp()
        {
            return View("~/Views/SetIp/SetIp.cshtml", new SetIpViewModel());
        }

        [HttpPost("set-ip")]
        public IActionResult SetIp([FromForm] SetIpViewModel model)
        {
            _logger.LogInformation("Incoming request content type: " + Request.ContentType);
            _logger.LogInformation("Form Data: IpAddress = " + model.IpAddress);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return View("~/Views/SetIp/SetIp.cshtml", model);
            }

            if (PingHost(model.IpAddress))
            {
                _sshStateService.Host = model.IpAddress; // Set the host in the shared state service
                ViewBag.Message = "Robot is reachable and IP address set successfully.";
            }
            else
            {
                _logger.LogWarning("Cannot reach the robot at the provided IP address");
                ModelState.AddModelError("", "Cannot reach the robot at the provided IP address.");
            }

            return View("~/Views/SetIp/SetIp.cshtml", model);
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                var result = _sshService.TestConnection();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("start-docker")]
        public IActionResult StartDocker()
        {
            try
            {
                var result = _robotService.ExecuteCommand(1); // Assuming functionId 1 is the Docker start command
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(nameOrAddress, 1000); // 1000ms timeout
                pingable = reply.Status == IPStatus.Success;
                _logger.LogInformation($"Ping status to {nameOrAddress}: {reply.Status}");
            }
            catch (PingException ex)
            {
                _logger.LogError($"PingException: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
            }

            return pingable;
        }

        [HttpGet("simple-test")]
        public IActionResult SimpleTest()
        {
            return View("SimpleTest");
        }

        [HttpPost("simple-test")]
        public IActionResult SimpleTest([FromForm] string testData)
        {
            _logger.LogInformation("Simple test data: " + testData);
            return Ok("Received: " + testData);
        }
    }
}
