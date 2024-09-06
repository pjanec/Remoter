using System;
using System.Collections.Generic;

namespace Remoter
{
	public class Computer
    {
        public Config.Computer Conf;
        
        public string IP => Conf.IP;
        public string UserName;
        public string Password;

        public bool IsRemote => _isRemoteFunc();
        public bool AlwaysLocal => Conf.AlwaysLocal??false;
        public string Label => Conf.Label;
        
        public string Station => Conf.Station;
        public string Group => Conf.Group;

        Func<bool> _isRemoteFunc;
        
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

        public Computer( Func<bool> isRemoteFunc )
        {
            _isRemoteFunc = isRemoteFunc;
        }
    }
}
