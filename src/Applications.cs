using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace Remoter
{
    public class App
    {
        public string Name; // service id
        public Image Image; // grid icon
        public AppDef AppDef; // app to run when service is chosen for starting
        public Func<Computer, Launcher> LauncherBuilder; // Called before launching. Can prepare necessary config files etc.
    }

    public static class Applications
	{
        static public List<App> Apps = new List<App>();
        static public List<string> ServiceNames = new List<string>();
        //static public App VNC;
        //static public App RDP;
        //static public App SSH;
        //static public App WinSCP;

        static public List<App> ServiceApps => (from x in ServiceNames let app = GetByName(x) where app != null select app).ToList();

        static Launcher DefaultLauncherBuilder( Computer comp, App app )
        {
            var vars = new Dictionary<string,string>();
            var svc = comp.Cfg.Services.Find( (x) => x.Name == app.Name );
            vars["PORT"] = svc.Port.ToString();
            return new Launcher( app.AppDef, null, vars );
        }

        static Config.Applications _cfg;

        
        static Config.App FindConfig( string name )
        {
            foreach( var c in _cfg.Apps )
            {
                if( c.Name == name ) return c;
            }
            return null;
        }

        static void LoadConfig( string fileName )
        {
            _cfg = JsonConvert.DeserializeObject<Config.Applications>( System.IO.File.ReadAllText( fileName ) );
            ServiceNames = _cfg.Services;

            foreach( var def in _cfg.Apps )
            {
                var a = new App();
                a.Name = def.Name;
                a.AppDef = new AppDef()
                {
                    ExeFullPath = def.ExeFullPath,
                    CmdLineArgs = def.CmdLineArgs,
                    StartupDir = def.StartupDir,
                };
                
                try   { a.Image = new Bitmap( $"Icons\\{def.Name}.png" ); }
                catch { a.Image = Resource1.Empty; }

                a.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, a ); };

                Register( a );
            }

        }

        //static void UpdateSvcFromConfig( App app )
        //{
        //    var cfg = FindConfig( app.Name );
        //    if( cfg == null ) return;
        //    if( cfg.ExeFullPath!=null ) app.AppDef.ExeFullPath = cfg.ExeFullPath;
        //    if( cfg.CmdLineArgs!=null ) app.AppDef.CmdLineArgs = cfg.CmdLineArgs;
        //    if( cfg.StartupDir!=null ) app.AppDef.StartupDir = cfg.StartupDir;
        //}

        static void Register( App app )
        {
            //UpdateSvcFromConfig( app );
            Apps.Add( app );

        }
        
        static Applications()
        {
            LoadConfig("apps.json");


            //VNC = new App()
            //{
            //    Name = "VNC",
            //    Image = Resource1.VNC,
            //    AppDef = new AppDef()
            //    {
            //        ExeFullPath = "tvnviewer.exe",
            //        CmdLineArgs = "127.0.0.1:%PORT%",
            //        StartupDir = ""
            //    }
            //};
            //VNC.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, VNC ); };
            //Register( VNC );
            
            //RDP = new App()
            //{
            //    Name = "RDP",
            //    Image = Resource1.RDP,
            //    AppDef = new AppDef()
            //    {
            //        ExeFullPath = @"%windir%\system32\mstsc.exe",
            //        CmdLineArgs = "/v:127.0.0.1:%PORT%",
            //        StartupDir = ""
            //    }
            //};
            //RDP.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, RDP ); };
            //Register( RDP );

            //SSH = new App()
            //{
            //    Name = "SSH",
            //    Image = Resource1.SSH,
            //    AppDef = new AppDef()
            //    {
            //        ExeFullPath = "ssh.exe",
            //        CmdLineArgs = "127.0.0.1:%PORT%",
            //        StartupDir = ""
            //    }
            //};
            //SSH.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, SSH ); };
            //Register( SSH );

            //WinSCP = new App()
            //{
            //    Name = "WinSCP",
            //    Image = Resource1.SCP,
            //    AppDef = new AppDef()
            //    {
            //        ExeFullPath = "winscp.exe",
            //        CmdLineArgs = "127.0.0.1:%PORT%",
            //        StartupDir = ""
            //    }
            //};
            //WinSCP.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, WinSCP ); };
            //Register( WinSCP );

        }

		public static App GetByName( string name )
		{
            return  Apps.Find( (x) => x.Name == name );
		}

	}

}
