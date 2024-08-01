using System.Threading.Tasks;

namespace RobotInterface.Services
{
    public interface IWebSocketService
    {
        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Sends a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        Task SendMessageAsync(string message);

        /// <summary>
        /// Receives a message from the WebSocket server.
        /// </summary>
        /// <returns>The received message.</returns>
        Task<string> ReceiveMessageAsync();

        /// <summary>
        /// Closes the WebSocket connection.
        /// </summary>
        Task CloseAsync();
    }
}
