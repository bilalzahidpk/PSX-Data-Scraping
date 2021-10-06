using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using IPT_ASSIGNMENT_Q2_SCRAPING;
using System.Configuration;


namespace IPT_ASSIGNMENT2_Q2
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private EventLog eventLog1;
        private string FilePath = ConfigurationManager.AppSettings.Get("FilePath");
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
            CreateXML.CreateXMLFile(FilePath);
            _timer = new Timer(10 * 60 * 1000);
        _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        _timer.Start();
        }
    private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {

        _timer.Stop();
        try
        {
                eventLog1.WriteEntry("Downloading File");
            
                CreateXML.CreateXMLFile(FilePath);
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
    }
}
