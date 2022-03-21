using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace Remoter
{

	public static class Applications
	{
        static public List<App> Apps = new List<App>();

        /// <summary>
        /// Union of all services
        /// </summary>
        //static public List<string> ServiceNames = new List<string>();

        //static public List<GridApp> GridApps => (from x in ServiceNames let app = ByName(x) where app != null select app).ToList();

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
                var a = LoadAppConfig( def );
                Register( a );
            }

        }

        static App LoadAppConfig( Config.App def )
        {
            var a = new App();
            a.LoadFromConfig( def );

            return a;
        }


        static void Register( App app )
        {
            Apps.Add( app );

        }
        
        static Applications()
        {
            LoadConfig("apps.json");
        }

		public static App ByName( string name )
		{
            return  Apps.Find( (x) => x.Name == name );
		}

	}

}
