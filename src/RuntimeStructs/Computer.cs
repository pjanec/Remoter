using System.Collections.Generic;

namespace Remoter
{
	public class Computer
    {
        public Config.Computer Conf;
        
        public string IP => Conf.IP;
        public bool BehindGateway => Conf.BehindGateway;
        public string UserName => Conf.UserName;
        public string Password => Conf.Password;
        
        /// <summary>
        /// Services running on a computer
        /// </summary>
        public List<Service> Services = new List<Service>();

        /// <summary>
        /// Apps to be started when clicked on the icon in the grid
        /// </summary>
        public List<GridApp> Apps = new List<GridApp>();
        
        /// <summary>
        /// What grid row this computer is shown in
        /// </summary>
        public int GridRowIdx;
    }
}
