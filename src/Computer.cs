using System.Collections.Generic;

namespace Remoter
{
	public class Computer
    {
        public Config.Computer Cfg;
        public string IP => Cfg.IP;
        public string Label => Cfg.Label;
        public List<Service> Services = new List<Service>();
        public int GridRowIdx;
    }

}
