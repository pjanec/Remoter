using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace Remoter
{
    /// <summary>
    /// Application that can be started by clicking in the grid
    /// </summary>
    public class GridApp
    {
        public string Name; 
        public string Service; // service id
        public Image Image; // grid icon
        public AppDef AppDef; // app to run when service is chosen for starting
        public Func<Computer, Launcher> LauncherBuilder; // Called before launching. Can prepare necessary config files etc.
    }

    public static class Applications
	{
        static public List<GridApp> Apps = new List<GridApp>();

        /// <summary>
        /// Union of all services
        /// </summary>
        //static public List<string> ServiceNames = new List<string>();

        //static public List<GridApp> GridApps => (from x in ServiceNames let app = ByName(x) where app != null select app).ToList();

        static Launcher DefaultLauncherBuilder( Computer comp, GridApp app )
        {
            var vars = new Dictionary<string,string>();
            var svc = comp.Conf.Services.Find( (x) => x.Name == app.Service );
            if( svc != null )
            {
                vars["SVC_PORT"] = svc.Port.ToString();
                vars["SVC_USERNAME"] = svc.UserName;
                vars["SVC_PASSWORD"] = svc.Password;
            }
            vars["APP_NEW_GUID"] = Guid.NewGuid().ToString();
            return new Launcher( app.AppDef, null, vars );
        }

        static Config.Application _cfg;

        
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
            _cfg = JsonConvert.DeserializeObject<Config.Application>( System.IO.File.ReadAllText( fileName ) );
            //ServiceNames = _cfg.Services;

            foreach( var def in _cfg.Apps )
            {
                var a = new GridApp();
                a.Name = def.Name;
                a.Service = def.Service;
                a.AppDef = new AppDef()
                {
                    ExeFullPath = def.ExeFullPath,
                    CmdLineArgs = def.CmdLineArgs,
                    StartupDir = def.StartupDir,
                };
                
                try   { a.Image = new Bitmap( def.IconFile ); }
                catch { a.Image = Resource1.Empty; }

                a.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, a ); };

                Register( a );
            }

        }

        static void Register( GridApp app )
        {
            Apps.Add( app );

        }
        
        static Applications()
        {
            LoadConfig("apps.json");
        }

		public static GridApp ByName( string name )
		{
            return  Apps.Find( (x) => x.Name == name );
		}

	}

}
