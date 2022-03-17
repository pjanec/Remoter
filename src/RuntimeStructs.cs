using System.Collections.Generic;

namespace Remoter
{
    // 
    /// <summary>
    /// App using the forwarded port of some network service, for example VNC viewer
    /// Launched when clicking on the grid
    /// </summary>
    public class ServiceApp
    {
        public string Name => App.Name;
        public Config.Service ServiceConf;
        public int LocalPort; // what port number we will use when connecting via 127.0.0.1:PORT

        // app to be started when we click the icon in the grid
        public int GridColIndex;
        public App App;
        public Launcher Launcher; // non-nul if service already running
    }

	public class Computer
    {
        public Config.Computer Cfg;
        
        public string IP => Cfg.IP;
        
        /// <summary>
        /// Apps to be started when clicked on the icon in the grid
        /// </summary>
        public List<ServiceApp> Services = new List<ServiceApp>();
        
        /// <summary>
        /// What grid row this computer is shown in
        /// </summary>
        public int GridRowIdx;
    }

    public class Context
    {
    }


}
