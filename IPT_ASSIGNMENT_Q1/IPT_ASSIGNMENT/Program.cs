using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IPT_ASSIGNMENT
{

    public class DownloadNewFile
    {
        public static void DownloadFile(string remoteFilename, string localFilename)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient client = new WebClient();
            client.DownloadFile(remoteFilename, localFilename);
        }
        static void Main(string[] args)
        {
            String fullPath = @"" + Path.Combine(args[1], "Summary" + DateTime.Now.ToString("ddMMMMy") + ".html");
            Console.WriteLine(fullPath);
            DownloadFile(args[0], fullPath);
        }
    }
}
