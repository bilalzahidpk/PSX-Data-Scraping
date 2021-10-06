using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;

namespace IPT_ASSIGNMENT2_Q1
{
    public partial class Service1 : ServiceBase
    {

        private Timer _timer;

        public Service1()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Windows Service Started");
            DownloadFile();
            _timer = new Timer(5 * 60 * 1000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();

        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            _timer.Stop();
            try
            {
                eventLog1.WriteEntry("Downloading File");
                DownloadFile();

            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("This is my error " + ex.Message);
            }
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
            eventLog1.WriteEntry("Windows Service Stopped");
        }
        public void DownloadFile()
        {
            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                string FileName = ConfigurationManager.AppSettings.Get("FileName");
                string Url = ConfigurationManager.AppSettings.Get("Url");
                string DestinationFileLocation = ConfigurationManager.AppSettings.Get("DestinationFileLocation");
                myProcess.StartInfo.FileName = FileName;
                myProcess.StartInfo.Arguments = Url + " " + DestinationFileLocation;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}


