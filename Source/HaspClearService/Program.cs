using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ArmoSystems.ArmoGet.HaspClearService
{
    internal static class Program
    {
        private static void Main()
        {
            SetupLog();
            var newService = new HaspClearService();
            try
            {
                RunWhileDebugging( newService );
                ServiceBase.Run( new ServiceBase[] { newService } );
            }
            catch ( Exception ex )
            {
                EventLog.WriteEntry( newService.ServiceName, ex.Message, EventLogEntryType.Error );
            }
        }

        private static void SetupLog()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            config.AddTarget( "file", fileTarget );

            // Step 3. Set target properties 
            fileTarget.FileName = Path.Combine( Path.GetTempPath(), "HaspClearService.txt" );
            fileTarget.Layout = "${longdate}|${message}";

            var rule = new LoggingRule( "*", LogLevel.Debug, fileTarget );
            config.LoggingRules.Add( rule );

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        [Conditional( "DEBUG" )]
        private static void RunWhileDebugging( HaspClearService newService )
        {
            newService.DebugStart();
            //Thread.Sleep( 30 * 1000 );
            Thread.Sleep( 100 * 60000 );
            newService.DebugStop();
        }
    }
}