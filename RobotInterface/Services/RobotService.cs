using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RobotInterface.Data;
using RobotInterface.Models;

namespace RobotInterface.Services
{
    public class RobotService : IRobotService
    {
        private readonly RobotInterfaceContext _context;
        private readonly ISshService _sshService;
        private readonly ILogger<RobotService> _logger;

        public RobotService(RobotInterfaceContext context, ISshService sshService, ILogger<RobotService> logger)
        {
            _context = context;
            _sshService = sshService;
            _logger = logger;
        }

        public string ExecuteCommand(int functionId)
        {
            _logger.LogInformation($"Starting execution of function {functionId}");

            var function = _context.Function
                .Include(f => f.FunctionCommands)
                .ThenInclude(fc => fc.Command)
                .FirstOrDefault(f => f.Id == functionId);

            if (function == null)
            {
                _logger.LogError($"Function {functionId} not found");
                throw new Exception("Function not found");
            }

            if (function.FunctionCommands.Count != 1)
            {
                _logger.LogError($"Function {functionId} should have exactly one command");
                throw new Exception("Function should have exactly one command");
            }

            var functionCommand = function.FunctionCommands.First();
            var command = functionCommand.Command;
            if (command == null)
            {
                _logger.LogError($"Command for function {functionId} not found");
                throw new Exception("Command not found");
            }

            // Find the latest running Docker container
            string findLatestContainerCommand = "docker ps -q --latest";
            _logger.LogInformation($"Finding latest Docker container: {findLatestContainerCommand}");
            var containerId = _sshService.ExecuteCommand(findLatestContainerCommand).Trim();
            _logger.LogInformation($"Latest Docker container ID: {containerId}");

            if (string.IsNullOrEmpty(containerId))
            {
                // Start Docker container if not running
                string startDockerCommand = "sh ros2_humble.sh";
                _logger.LogInformation($"Executing Docker start command: {startDockerCommand}");
                var startDockerOutput = _sshService.ExecuteCommand(startDockerCommand);
                _logger.LogInformation($"Docker start command output: {startDockerOutput}");

                // Find the latest container again after starting
                containerId = _sshService.ExecuteCommand(findLatestContainerCommand).Trim();
                _logger.LogInformation($"Latest Docker container ID after starting: {containerId}");
            }

            // Single command to create, make executable, and run the script inside the Docker container
            string combinedCommand = $@"
docker exec -i {containerId} bash -c 'cat << ""EOF"" > /root/command_{functionId}.sh
#!/bin/bash
echo ""Script started""
source /opt/ros/humble/setup.bash
echo ""ROS setup sourced""
cd /root/yahboomcar_ws
echo ""Changed directory""
source install/setup.bash
echo ""Workspace setup sourced""
{command.CommandName} > /root/command_{functionId}.log 2>&1 &
echo ""Command started in background""
EOF
chmod +x /root/command_{functionId}.sh
/root/command_{functionId}.sh
'";

            _logger.LogInformation($"Executing combined command inside Docker container: {combinedCommand}");
            var commandOutput = _sshService.ExecuteCommand(combinedCommand.Replace("\r", ""));
            _logger.LogInformation($"Combined command output: {commandOutput}");

            _logger.LogInformation($"Completed execution of function {functionId} with results: {commandOutput}");

            return commandOutput;
        }
    }
}
