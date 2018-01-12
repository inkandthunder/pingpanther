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
            Ping("csstech");
            Console.ReadKey();
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
                        Console.WriteLine("Pinging with server: " + reply.Address);
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        static void Ping(string hostname) //ping function
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(hostname, 100);
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Pinged " + hostname + " at " + reply.Address + " Successfully. \t Time: " + reply.RoundtripTime + " ms \r\n");
                    }
                    else if (reply.Status == IPStatus.TimedOut) //Problem with the pings to be too frequently timed out, so a "fix" or "hack" around this.
                    {
                        Console.WriteLine("Connection time out. Connection retried for " + hostname + "\r\n");
                        //PingReply reply2 = ping.Send(hostname, 100);
                        //Console.WriteLine("Pinged " + hostname + " at " + reply2.Address + " Successfully. \t Time: " + reply2.RoundtripTime + " ms \r\n");
                    }
                    else
                    {
                        Console.WriteLine("Couldn't ping " + hostname + "; Error: " + reply.Status + ".\r\n");
                        Console.WriteLine(reply.Status);
                    }
                }
                catch (Exception ex)
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
