using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace Remoter
{
	public class Session
    {
        public string Name;

        public Config.Session Conf;

        public Gateway Gateway;

        public List<Computer> Computers = new List<Computer>();
        public Forwarder Forwarder;

        public bool IsPortForwarding => Forwarder.IsRunning;


        public Session( string fileName )
        {
            LoadSessionConfig( fileName );
            Name = string.IsNullOrEmpty( Conf.Name ) ? System.IO.Path.GetFileNameWithoutExtension( fileName ) : Conf.Name;
        }

        public void Dispose()
        {
            KillApps();
            Forwarder?.Dispose();
        }
        
        void LoadSessionConfig( string fileName )
        {
            Conf = JsonConvert.DeserializeObject<Config.Session>( System.IO.File.ReadAllText( fileName ) );

            Gateway = new Gateway()
            {
                ExternalIP = Conf.Gateway.ExternalIP,
                InternalIP = Conf.Gateway.InternalIP,
                Port = Conf.Gateway.Port ?? 22,
                UserName = Conf.Gateway.UserName ?? Conf.DefaultCredentials.UserName,
                Password = Conf.Gateway.Password ?? Conf.DefaultCredentials.Password,
            };


            foreach( var compConf in Conf.Computers )
            {
                var comp = new Computer(
                    () => Forwarder.IsRunning ? !compConf.AlwaysLocal : false  // if forwarder not working, consider all computers local
                )
                {
                    Conf = compConf,
                    UserName = compConf.UserName ?? Conf.DefaultCredentials.UserName,
                    Password = compConf.Password ?? Conf.DefaultCredentials.Password,
                };

                Computers.Add( comp );
            }


            // fill columns for each computer in the session
            foreach( var comp in Computers )
            {
                foreach( var svcConf in comp.Conf.Services )
                {
                    comp.Services.Add(
                        new Service(
                            comp,
                            // local
                            comp.IP,
                            svcConf.Port,
                            // remote
                            "127.0.0.1",
                            ++GlobalContext.Instance.LocalPortBase
                        )
                        {
                            Conf = svcConf,
                            UserName = svcConf.UserName ?? comp.UserName,
                            Password = svcConf.Password ?? comp.Password,
                        }
                    );
                }

                foreach( var appLink in comp.Conf.Apps )
                {
                    // load app hierarchically (default first, overwrite with our app settings)
                    var appTempl = Applications.ByName( appLink.Name ); //load default first
                    var app = appTempl == null ? new App() : (App) appTempl.Clone(); // if no default, start from scratch
                    app.LoadFromConfig( appLink ); // overwrite with our settings
                    if( app.Name == null ) continue; // nothing defined

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
