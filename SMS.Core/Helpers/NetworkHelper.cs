using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Helpers
{
    public class NetworkHelper
    {
        public static string GetLocalIpAddress()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .First(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
        }

        public static string GetPublicIpAddress()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    return webClient.DownloadString("https://api.ipify.org");
                }
                catch
                {
                    return "Unable to retrieve public IP address";
                }
            }
        }
    }
}
