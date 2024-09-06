using Newtonsoft.Json;

namespace Remoter
{
	public class GlobalContext
	{
		public static GlobalContext Instance;
        
        /// <summary>
        /// Local ports used with IP 127.0.0.1 when connecting to forwarded ports
        /// Incremented for each port-forwaded service
        /// </summary>
        public int LocalPortBase = 40000;


		public GlobalContext()
		{
			Instance = this;
		}
	}


}
