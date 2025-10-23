using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.NETFramework.Helpers.Zatca.Logging
{
    public class ZatcaLogger
    {
        public static void Start()
        {
            LogMessages = true;
        }
        public static void Stop()
        {
            LogMessages = false;
        }
        private static bool LogMessages { set; get; }
        public static void Log(string message)
        {
            LogToFile(DateTime.Now.ToString("hh:mm:ss") + " : " + message);
        }

        public static void Log(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message.ToString());
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine(ex.InnerException?.Message);
            sb.AppendLine(ex.InnerException?.StackTrace);

            LogToFile(sb.ToString());
        }

        private static void LogToFile(string msg)
        {
            if (LogMessages)
            {
                var path = Environment.CurrentDirectory + "/Logs/" + DateTime.Now.ToString("ddMMyyyyhh") + ".txt";
                if (!Directory.Exists(Environment.CurrentDirectory + "/Logs/"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/Logs/");
                }
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(msg);
                }
            }
        }
    }
}
