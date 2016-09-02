using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;

namespace ArmoSystems.ArmoGet.HaspClearService
{
    internal sealed partial class HaspClearService : ServiceBase
    {
        private ServiceHost host;

        public HaspClearService()
        {
            InitializeComponent();
        }

        protected override void OnStart( string[] args )
        {
            try
            {
                host = new ServiceHost( typeof ( WcfService ) );
                host.Open();
            }
            catch ( Exception ex )
            {
                EventLog.WriteEntry( ex.Message, EventLogEntryType.Information );
            }
        }

        protected override void OnStop() => host.Close();

        public void DebugStart() => OnStart( null );

        public void DebugStop() => OnStop();
    }
}