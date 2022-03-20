using System.Collections.Generic;

namespace Remoter
{
	public class Computer
    {
        public Config.Computer Conf;
        
        public string IP => Conf.IP;
        
        /// <summary>
        /// Services running on a computer
        /// </summary>
        public List<Service> Services = new List<Service>();

        /// <summary>
        /// Apps to be started when clicked on the icon in the grid
        /// </summary>
        public List<ConsumerApp> Apps = new List<ConsumerApp>();
        
        /// <summary>
        /// What grid row this computer is shown in
        /// </summary>
        public int GridRowIdx;
    }
}
