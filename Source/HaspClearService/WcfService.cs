using System;
using System.Collections.Specialized;
using System.Net;
using System.ServiceModel;
using System.Threading;
using Newtonsoft.Json;
using NLog;

namespace ArmoSystems.ArmoGet.HaspClearService
{
    [ServiceBehavior( ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single )]
    public sealed class WcfService : IService
    {
        private const int IntervalBeetweenCalls = 35;
        private static DateTime _lastCall = DateTime.Now.Subtract( TimeSpan.FromSeconds( 5 ) );

        private readonly Logger logger = LogManager.GetLogger( "WcfService" );

        public void RestartSLM( string computerName )
        {
            logger.Info( computerName );
            logger.Info( _lastCall.ToLongTimeString() );
            var timeout = ( int ) ( IntervalBeetweenCalls - ( DateTime.Now - _lastCall ).TotalSeconds );
            logger.Info( timeout );
            if ( timeout > 0 )
            {
                logger.Info( "Timeout: " + timeout );
                Thread.Sleep( timeout * 1000 );
            }

            logger.Info( "Сбрасываю сессии" );
            while ( true )
            {
                var session =
                    JsonConvert.DeserializeObject< RootObject >( new WebClient().DownloadString( "http://localhost:1947/_int_/tab_sessions.html?haspid=0&featureid=-1&vendorid=0&productid=0&filterfrom=1&filterto=100" ),
                        new JsonSerializerSettings { CheckAdditionalContent = false } ).sid;
                if ( session == null )
                    break;

                RemoveSession( session );
            }

            logger.Info( "Сессии сброшены" );

            _lastCall = DateTime.Now;
        }

        private static void RemoveSession( string id )
        {
            using ( var client = new WebClient() )
            {
                client.UploadValues( "http://localhost:1947/_int_/action.html", new NameValueCollection { ["deletelogin"] = id } );
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class RootObject
        {
            // ReSharper disable UnusedMember.Local
            public string index { get; set; }
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string sid { get; set; }
            public string acc { get; set; }
            public string vendorid { get; set; }
            public string haspid { get; set; }
            public string haspname { get; set; }
            public string prid { get; set; }
            public string productname { get; set; }
            public string prv { get; set; }
            public string fid { get; set; }
            public string fn { get; set; }
            public string cli { get; set; }
            public string usr { get; set; }
            public string scrn { get; set; }
            public string mch { get; set; }
            public string pid { get; set; }
            public string seats { get; set; }
            public string lt { get; set; }
            public string rt { get; set; }
            public string lm_version { get; set; }
            public string lm_version_full { get; set; }
            public string lm_build { get; set; }
            public string api_version { get; set; }
            public string ddis { get; set; }
            // ReSharper restore UnusedMember.Local
        }
    }
}