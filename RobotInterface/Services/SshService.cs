using Renci.SshNet;
using System;
using Microsoft.Extensions.Logging;

namespace RobotInterface.Services
{
    public class SshService : ISshService
    {
        private string _host;
        private readonly string _username = "pi";
        private readonly string _password = "yahboom";
        private readonly ILogger<SshService> _logger;
        private readonly ISshStateService _sshStateService;

        public SshService(ILogger<SshService> logger , ISshStateService sshStateService)
        {
            _logger = logger;
            _sshStateService = sshStateService;
        }

        public void SetHost(string host)
        {
            _sshStateService.Host = host;
        }

        public string GetHost()
        {
            return _sshStateService.Host;
        }

        public string TestConnection()
        {
            if (string.IsNullOrEmpty(_sshStateService.Host))
            {
                throw new ArgumentNullException(nameof(_sshStateService.Host), "Host cannot be null or empty.");
            }

            try
            {
                _logger.LogInformation($"Connecting to SSH server {_sshStateService.Host} with user {_username}");

                using var client = new SshClient(_sshStateService.Host, _username, _password);
                client.Connect();
                _logger.LogInformation($"Connected to SSH server {_sshStateService.Host}");

                client.Disconnect();
                return "SSH connection successful.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error connecting to SSH server: {ex.Message}");
                throw;
            }
        }

        public string ExecuteCommand(string command, int timeoutSeconds = 300)
        {
            if (string.IsNullOrEmpty(_sshStateService.Host))
            {
                throw new ArgumentNullException(nameof(_sshStateService.Host), "Host cannot be null or empty.");
            }

            try
            {
                _logger.LogInformation($"Connecting to SSH server {_sshStateService.Host} with user {_username}");

                var connectionInfo = new PasswordConnectionInfo(_sshStateService.Host, _username, _password)
                {
                    Timeout = TimeSpan.FromSeconds(timeoutSeconds)
                };

                using var client = new SshClient(connectionInfo);
                client.Connect();
                _logger.LogInformation($"Connected to SSH server {_sshStateService.Host}");

                using var cmd = client.CreateCommand(command);
                cmd.CommandTimeout = TimeSpan.FromSeconds(timeoutSeconds);
                var result = cmd.Execute();
                var error = cmd.Error;

                _logger.LogInformation($"Command executed: {command}");
                _logger.LogInformation($"Command result: {result}");
                _logger.LogInformation($"Command error: {error}");

                client.Disconnect();
                return string.IsNullOrEmpty(result) ? error : result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing command {command}: {ex.Message}");
                throw;
            }
        }
    }
}
