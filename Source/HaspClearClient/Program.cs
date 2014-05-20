using System;
using ArmoSystems.ArmoGet.HaspClearClient.DSServiceReference;

namespace ArmoSystems.ArmoGet.HaspClearClient
{
    internal static class Program
    {
        [STAThread]
        private static int Main()
        {
            try
            {
                new ServiceClient().RestartSLM();
                return 0;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return -1;
            }
        }
    }
}