using System;
using ArmoSystems.ArmoGet.HaspClearClient.HaspClearServiceReference;

namespace ArmoSystems.ArmoGet.HaspClearClient
{
    internal static class Program
    {
        [STAThread]
        private static int Main()
        {
            // SetupLog();
            //var logger = LogManager.GetLogger( "WcfService" );
            try
            {
                //  logger.Info( "Вызываю сервис" );
                new ServiceClient().RestartSLM( Environment.MachineName );
                //logger.Info( "Успешно сброшены лицензии" );
                return 0;
            }
            catch ( Exception ex )
            {
                //logger.Info( ex.ToString() );
                //Console.WriteLine( ex.ToString() );
                return -1;
            }
        }

/*
        private static void SetupLog()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            config.AddTarget( "file", fileTarget );

            // Step 3. Set target properties 
            fileTarget.FileName = @"D:\Temp\HaspClearService.txt";
            fileTarget.Layout = "${longdate}|${message}";

            var rule = new LoggingRule( "*", LogLevel.Debug, fileTarget );
            config.LoggingRules.Add( rule );

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }*/
    }
}