using Newtonsoft.Json;

namespace Remoter
{
	public class GlobalContext
	{
		public static GlobalContext Instance;
        public Config.Application AppConf;
        
        /// <summary>
        /// Local ports used with IP 127.0.0.1 when connecting to forwarded ports
        /// Incremented for each port-forwaded service
        /// </summary>
        public int LocalPortBase = 40000;


        public void LoadApplicationConfig( string fileName )
        {
            AppConf = JsonConvert.DeserializeObject<Config.Application>( System.IO.File.ReadAllText( fileName ) );
        }

		public GlobalContext()
		{
			Instance = this;
		}
	}


}
