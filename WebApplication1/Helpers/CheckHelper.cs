using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace WebApplication1.Helpers
{
    public class CheckHelper
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
    }
}
