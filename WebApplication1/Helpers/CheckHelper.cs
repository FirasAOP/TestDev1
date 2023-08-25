using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using WebApplication1.Hub;

namespace WebApplication1.Helpers
{
    public class CheckHelper
    {
        WebSocketHub _webSocketHub;
   
        public CheckHelper(WebSocketHub webSocketHub)
        {
            _webSocketHub = webSocketHub;
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public async Task CheckConnectionJobAsync()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            bool connectionStatusInit = false;
            while (await timer.WaitForNextTickAsync())
            {
               var  connectionStatus = IsConnectedToInternet();
                //if (connectionStatus !=connectionStatusInit) 
                //{
                // if a connection updated, send new data to all sockets
              
                await _webSocketHub.SendAll(JsonConvert.SerializeObject(new {status = connectionStatus}));
                //}
                connectionStatusInit = connectionStatus;
               
            }
        }
        

      
    }
}
