using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RobotInterface.Services;
using System.Threading.Tasks;

namespace RobotInterface.Hubs
{
    public class RobotHub : Hub
    {
        private readonly IRobotService _robotService;
        private readonly ISshService _sshService;
        private readonly ISshStateService _sshStateService;
        private readonly ILogger<RobotHub> _logger;

        public RobotHub(IRobotService robotService, ISshService sshService, ISshStateService sshStateService, ILogger<RobotHub> logger)
        {
            _robotService = robotService;
            _sshService = sshService;
            _sshStateService = sshStateService;
            _logger = logger;
        }

        public async Task SendCommand(int functionId)
        {
            _logger.LogInformation($"Received function ID: {functionId}");

            // Get the current host from the shared state service
            var host = _sshStateService.Host;
            _logger.LogInformation($"Current SSH host: {host}");

            if (string.IsNullOrEmpty(host))
            {
                _logger.LogError("SSH host is not set.");
                await Clients.All.SendAsync("ReceiveResponse", "SSH host is not set.");
                return;
            }

            // Set the host in the SSH service
            _sshService.SetHost(host);

            var result = _robotService.ExecuteCommand(functionId);
            _logger.LogInformation($"Command result: {result}");
            await Clients.All.SendAsync("ReceiveResponse", result);
        }
    }
}
