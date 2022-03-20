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

            Forwarder = new Forwarder( this );


            // fill columns for each computer in the session
            foreach( var comp in Computers )
            {
                foreach( var svcConf in comp.Conf.Services )
                {
                    comp.Services.Add(
                        new Service()
                        {
                            Conf = svcConf,
                            LocalPort = ++GlobalContext.Instance.LocalPortBase,
                        }
                    );
                }

                foreach( var appConf in comp.Conf.Apps )
                {
                    // find service conf
                    var serviceConf = comp.Conf.Services.Find( (x) => x.Name == appConf.Service );
                    var app = Applications.ByName( appConf.Name );

                    comp.Apps.Add(
                        new ConsumerApp()
                        {
                            ServiceConf = serviceConf,
                            App = app,
                        }
                    );
                }
            }
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
