using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotInterface.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly ClientWebSocket _webSocket;
        private readonly Uri _uri;

        public WebSocketService(string url)
        {
            _uri = new Uri(url);
            _webSocket = new ClientWebSocket();
        }

        public async Task ConnectAsync()
        {
            await _webSocket.ConnectAsync(_uri, CancellationToken.None);
        }

        public async Task SendMessageAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        public async Task CloseAsync()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}
