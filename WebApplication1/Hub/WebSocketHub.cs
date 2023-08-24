using Newtonsoft.Json;
using System.Data;
using System.Net.WebSockets;
using System.Text;

namespace WebApplication1.Hub
{
    public class WebSocketHub
    {
        private List<WebSocket> _webSocketList = new List<WebSocket>();

        // add a socket to list
        public async void AddSocket(WebSocket webSocket)
        {
            try
            {
                if (webSocket == null) return;
                // _webSocketList this list is used asynchronously so when we want to use it we need to use lock
                lock (_webSocketList) _webSocketList.Add(webSocket);

                // if socket open send initial message
                if (webSocket.State == WebSocketState.Open)
                {
                    var messageObject = new { status = "Start" };
                    var messageString = JsonConvert.SerializeObject(messageObject);
                    var bytes = Encoding.UTF8.GetBytes(messageString);
                    await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                    //byte[] messageBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("{status:start}") );
                    //_ = webSocket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception exp)
            {
                //log 
            }
        }

        // remove a socket from list
        public void RemoveSocket(WebSocket webSocket)
        {
            lock (_webSocketList) _webSocketList.Remove(webSocket);
        }

        // send a message to all open sockets
        public async Task SendAll(string message)
        {
            try
            {
                List<WebSocket> webSocketList;
                lock (_webSocketList) webSocketList = _webSocketList;

                byte[] byteMessage = Encoding.UTF8.GetBytes(message);

                webSocketList.ForEach(f =>
                {
                    if (f.State == WebSocketState.Open)
                    {
                        f.SendAsync(new ArraySegment<byte>(byteMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                });
            }
            catch (Exception)
            {
                // log exp
            }
        }
    }
}
