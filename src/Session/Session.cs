using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace Remoter
{
	public class Session
    {
        public Config.Session Conf;

        public List<Computer> Computers = new List<Computer>();
        public Forwarder Forwarder;


        public Session( string fileName )
        {
            LoadSessionConfig( fileName );
        }

        public void Dispose()
        {
            KillApps();
            Forwarder?.Dispose();
        }
        
        void LoadSessionConfig( string fileName )
        {
            Conf = JsonConvert.DeserializeObject<Config.Session>( System.IO.File.ReadAllText( fileName ) );

            foreach( var c in Conf.Computers )
            {
                var comp = new Computer() { Conf = c };
                Computers.Add( comp );
            }


            // fill columns for each computer in the session
            foreach( var comp in Computers )
            {
                foreach( var svcConf in comp.Conf.Services )
                {
                    var port = comp.Conf.BehindGateway
                                ? ++GlobalContext.Instance.LocalPortBase
                                : svcConf.Port;

                    var ip = comp.Conf.BehindGateway
                                ? "127.0.0.1"
                                : comp.Conf.IP;

                    comp.Services.Add(
                        new Service()
                        {
                            Conf = svcConf,
                            IP = ip,
                            Port = port 
                        }
                    );
                }

                foreach( var appConf in comp.Conf.Apps )
                {
                    // find service conf
                    var serviceConf = comp.Conf.Services.Find( (x) => x.Name == appConf.Service );
                    var app = Applications.ByName( appConf.Name );

                    comp.Apps.Add(
                        new GridApp()
                        {
                            ServiceConf = serviceConf,
                            App = app,
                        }
                    );
                }
            }


            Forwarder = new Forwarder( this );
        }

        void KillApps()
        {
            foreach( var c in Computers )
            {
                foreach( var s in c.Apps )
                {
                    if( s.Launcher != null )
                    {
                        s.Launcher.Kill();
                    }
                }
            }
            // give time for services to dies
            Thread.Sleep(1000);
        }

        public void Start()
        {
            Forwarder.Start();
        }

    }


}
