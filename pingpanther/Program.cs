using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace pingpanther
{
    class Program
    {
        public static NameValueCollection AppSettings { get; }
        public static List<string> MachineList = new List<string> { };
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Loopback();
            StringSplitter(ReadSetting("hosts"));
            Ping(MachineList);
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

        static void StringSplitter(string input)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', ';', '\t' };
            MachineList = input.Split(delimiterChars).ToList();
        }

        static void Ping(List<string> hostname) //ping function
        {
            foreach (var host in hostname)
                {
                using (Ping ping = new Ping())
                {
                    try
                    {
                        PingReply reply = ping.Send(host, 100);
                        if (reply.Status == IPStatus.Success)
                        {
                            Console.WriteLine("Pinged " + hostname + " at " + reply.Address + " Successfully. \t Time: " + reply.RoundtripTime + " ms \r\n");
                            log.Info("Pinged " + hostname + ", " + reply.Address + " responded successfully in " + reply.RoundtripTime + " ms \r\n");
                        }
                        else if (reply.Status == IPStatus.TimedOut) //Problem with the pings to be too frequently timed out, so a "fix" or "hack" around this.
                        {
                            Console.WriteLine("Connection time out. Connection retried for " + hostname + "\r\n"); }
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
        }

      static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException ex)
            {
                log.Error("Error reading the app configuration", ex);
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public static void BuildEmailMessage()
        {
            var mail = new MailMessage();
            var smtp = new SmtpClient();
            var body = "<p>" + ReadSetting("body") + "<br><br>";
            foreach (var result in StalledFiles)
            {
                body += Path.GetFileName(result) + " : Created on " + File.GetCreationTime(result) + "<br>";
            }
            body += "</p>";

            try
            {
                foreach (var to in ReadSetting("to").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(to);
                }
                foreach (var cc in ReadSetting("cc").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.CC.Add(cc);
                }
            }
            catch (Exception ex)
            {
                log.Error("Missing email parameters in the app config", ex);
            }

            try
            {
                log.Info("Sending notification to distribution list");
                mail.From = new MailAddress(ReadSetting("from"));
                mail.Subject = ReadSetting("subject");
                mail.IsBodyHtml = true;
                mail.Body = string.Format(body);
                smtp.Port = int.Parse(ReadSetting("port"));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Host = ReadSetting("smtpHost");
                SendEmail(smtp, mail);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                log.Error("Email could not be delivered", ex);
            }
        }

        private static void SendEmail(SmtpClient smtp, MailMessage mail)
        {
            try
            {
                smtp.Send(mail);
                mail.Dispose();
            }
            catch (SmtpFailedRecipientsException ex)
            {
                log.Error("There was a problem sending the email", ex);
                mail.Dispose();
            }
            //mail.Dispose();
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
