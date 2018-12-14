using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace test1
{
    class Program
    {
        public static string GetLocalIp()
        {
            IPAddress localIp = null;

            try
            {
                IPAddress[] ipArray;
                ipArray = Dns.GetHostAddresses(Dns.GetHostName());
                localIp = ipArray.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            }
            catch (Exception ex)
            {
            }
            if (localIp == null)
            {
                localIp = IPAddress.Parse("127.0.0.1");
            }
            return localIp.ToString();
        }
        static void Main(string[] args)
        {
            Console.WriteLine(GetLocalIp());
        }
    }
}
