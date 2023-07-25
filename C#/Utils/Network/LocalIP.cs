using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Utils.Network
{
    public class LocalIP
    {
        public static string ParsingIP(string host) => host == "127.0.0.1" ? GetLocalIP() : host;

        public static string GetLocalIP(string mac = "")
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string localIP = "127.0.0.1";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => !n.Description.ToLower().Contains("virtual"))
                .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet || n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIP = ip.Address.ToString();
                            if (string.IsNullOrEmpty(mac) || mac == item.GetPhysicalAddress().ToString())
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return localIP;
        }

        public static bool IsLoacalIP(string ip)
        {
            var localIP = GetLocalIP();

            return ip == "127.0.0.1" ? true : localIP == ip ? true : false;
        }
    }
}
