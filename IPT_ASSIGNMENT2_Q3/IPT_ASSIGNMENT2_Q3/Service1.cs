using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;

namespace IPT_ASSIGNMENT2_Q3
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private EventLog eventLog1;
        private string SourceFolder = ConfigurationManager.AppSettings.Get("SourceFolder");
        private string DestinationFolder = ConfigurationManager.AppSettings.Get("DestinationFolder");
        public Service1()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Windows Service Started");
            GetDirectoriesAndGetOldFiles(@"" + SourceFolder, @"" + DestinationFolder);
            _timer = new Timer(15 * 60 * 1000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
            eventLog1.WriteEntry("Windows Service Stopped");
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            _timer.Stop();
            try
            {
                eventLog1.WriteEntry("Performing Task");
                GetDirectoriesAndGetOldFiles(@"" + SourceFolder, @"" + DestinationFolder);
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("This is my error " + ex.Message);
            }
            _timer.Start();
        }

        public static void GetDirectoriesAndGetOldFiles(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);
            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                FileInfo[] files = diSourceSubDir.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                if (files.Length == 1)
                {
                    File.Move(@"" + files[0].FullName, @"" + nextTargetSubDir.FullName + @"\" + files[0].Name);
                }
                else
                {
                    for (int i = 0; i < files.Length - 1; i++)
                    {
                        File.Move(@"" + files[i].FullName, @"" + nextTargetSubDir.FullName + @"\" + files[i].Name);
                    }
                }
            }
        }
    }
}
