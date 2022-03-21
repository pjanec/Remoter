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
                    var port = comp.BehindGateway
                                ? ++GlobalContext.Instance.LocalPortBase
                                : svcConf.Port;

                    var ip = comp.BehindGateway
                                ? "127.0.0.1"
                                : comp.Conf.IP;

                    comp.Services.Add(
                        new Service()
                        {
                            Conf = svcConf,
                            IP = ip,
                            Port = port,
                            UserName = svcConf.UserName ?? comp.UserName,
                            Password = svcConf.Password ?? comp.Password,
                        }
                    );
                }

                foreach( var appLink in comp.Conf.Apps )
                {
                    // find service conf
                    var app = Applications.ByName( appLink.Name );
                    if( app == null ) continue;

                    var svcName = appLink.Service ?? string.Empty;
                    if( string.IsNullOrEmpty( svcName ) )
                    {
                        svcName = app.Service ?? string.Empty;
                    }

                    comp.Apps.Add(
                        new GridApp()
                        {
                            Service = comp.Services.Find( (x) => x.Name == svcName ),
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

        public void Stop()
        {
            Forwarder.Stop();
        }
    }


}
