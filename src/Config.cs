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

        
        /// <summary>
        /// Network service runningn on a port we want to access via port forwarding
        /// </summary>
        public class Service
        {
            public string Name;
            public int Port;
            public string UserName;
            public string Password;
        }

        public class ConsumerApp
        {
            /// <summary>
            /// Type name of the app, used to reference this app in the session config
            /// </summary>
            public string Name;
            
            /// <summary>
            /// What service is this app using
            /// </summary>
            public string Service;
        }




        public class Computer
        {
            public string Label;
            public string IP;
            public List<Service> Services = new List<Service>(); // just those configured in the config
            public List<ConsumerApp> Apps = new List<ConsumerApp>(); // just those configured in the config
        }

        /// <summary>
        /// Application config (same for all sessions)
        /// </summary>
        public class Application
        {
            public List<App> Apps;
            //public List<string> Services;
        }

        public class App
        {
            public string Name;
            public string Service; // service we are using
            public string IconFile;
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
