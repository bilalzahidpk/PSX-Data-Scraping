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
using IPT_ASSIGNMENT_Q4_JSON;
using System.Configuration;

namespace IPT_ASSIGNMENT2_Q4
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private EventLog eventLog1;
        public static string GetDirectories = ConfigurationManager.AppSettings.Get("GetFiles");
        public static string DestinationFolder = ConfigurationManager.AppSettings.Get("DestinationFolder");
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
            Class1.GetJsonAndRemoveXML(GetDirectories, DestinationFolder);
            _timer = new Timer(20 * 60 * 1000);
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
                Class1.GetJsonAndRemoveXML(GetDirectories, DestinationFolder);
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("This is my error " + ex.Message);
            }
            _timer.Start();
        }

    }
}
