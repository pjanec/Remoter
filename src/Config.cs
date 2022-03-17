using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoter
{
    namespace Config
    {
        public class Session
        {
            public string Gateway;
            public string UserName;
            public string Password;
            public List<Computer> Computers;
        }

        
        public class Service
        {
            public string Name;
            public int Port;
        }

        public class AuthenticatedService : Service
        {
            public string UserName;
            public string Password;
        }

        public class VNC : AuthenticatedService
        {
        }

        public class RDP : AuthenticatedService
        {
        }

        public class SSH : AuthenticatedService
        {
        }

        public class WinSCP : AuthenticatedService
        {
        }

        public class Services
        {
            public VNC VNC;
            public RDP RDP;
            public SSH SSH;
            public WinSCP WinSCP;

            //List<Service> 

            public Service ByName(string name)
            {
                if( name=="VNC" ) return VNC;
                if( name=="RDP" ) return RDP;
                if( name=="SSH" ) return SSH;
                if( name=="WinSCP" ) return WinSCP;
                return null;
            }
        }



        public class Computer
        {
            public string Label;
            public string IP;
            public List<Service> Services = new List<Service>(); // just those configured in the config
        }

        public class Applications
        {
            public List<App> Apps;
            public List<string> Services;
        }

        public class App
        {
            public string Name;
            public string ExeFullPath;
            public string CmdLineArgs;
            public string StartupDir;
            //public bool AdoptIfAlreadyRunning;
            //public EWindowStyle WindowStyle = EWindowStyle.NotSet;
            //public Dictionary<string, string> EnvVarsToSet = new Dictionary<string, string>();
            //public Dictionary<string, string> LocalVarsToSet = new Dictionary<string, string>();
            //public string EnvVarPathToPrepend;
            //public string EnvVarPathToAppend;
            //public string PriorityClass; // idle, belownormal, normal, abovenormal, high, realtime; empty = normal
            //public bool KillTree;
        }

    }
}
