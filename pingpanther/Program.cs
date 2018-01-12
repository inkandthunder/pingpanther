using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Net;

namespace pingpanther
{
    class Program
    {
        public static NameValueCollection AppSettings { get; }
        public static List<string> ServerList = new List<string> { };
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Loopback();
        }

        static void Loopback()
        {
            using (var ping = new Ping())
            {
                var reply = ping.Send(IPAddress.Loopback);

                try
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        log.Info("Pinging with server: " + reply.Address);
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        //public static List<string> GetServers()
        //{
        //    public string Hosts = ReadSetting("hosts");
        //}

        //public static bool PingServer()
        //{

        //}
    }
}
