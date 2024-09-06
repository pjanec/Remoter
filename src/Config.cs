using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoter
{
    namespace Config
    {

        public class Credentials
        {
            public string UserName;
            public string Password;
        }

        public class Gateway
        {
            public string ExternalIP; // accessible from outside
            public int? Port;
            public string InternalIP; // internal IP of the gateway (to be used if not port forwarding)
            public string UserName;
            public string Password;
        }

        public class Session
        {
            public string Name;
            public Credentials DefaultCredentials;
            public Gateway Gateway;
            public List<Computer> ComputerTemplates;
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

            public void Merge( Service other )
            {
                if( other.UserName != null ) UserName = other.UserName;
				if( other.Password != null ) Password = other.Password;
                if( other.Port != 0 ) Port = other.Port;
			}
        }


        public class Computer
        {
            public string Template; // Label of a template configuration
            public string Group; // for display & sorting
            public string Station; // for display & sorting
            public string Label;  // for display & sorting
            public string IP; // internal ip (accessible from inner network)
            public string UserName;
            public string Password;

            
            /// <summary>
            /// Is the IP not accessible directly but just via the gateway
            /// </summary>
            public bool? AlwaysLocal;

            /// <summary>
            /// What network services are running on the computer
            /// </summary>
            public List<Service> Services = new List<Service>(); // just those configured in the config
            
            /// <summary>
            /// What apps will be shown in our toolbar for this computer.
            /// The apps can be associated with a service running on this computer;
            /// in such a case they can use the service IP and port (forwarded one of the computer is behind a gateway)
            /// </summary>
            public List<App> Apps = new List<App>(); // just those configured in the config


            public Computer Clone()
			{
				var res = new Computer();
				res.Merge( this );
				return res;
			}
            
            public void Merge( Computer other )
			{
                if( other.Group != null ) Group = other.Group;
                if( other.Station != null ) Station = other.Station;
				if( other.Label != null ) Label = other.Label;
				if( other.IP != null ) IP = other.IP;
				if( other.UserName != null ) UserName = other.UserName;
				if( other.Password != null ) Password = other.Password;
				if( other.AlwaysLocal.HasValue ) AlwaysLocal = other.AlwaysLocal;

				if( other.Services != null )
				{
					foreach( var svc in other.Services )
					{
                        var found = Services.Find( (x) => x.Name == svc.Name );
						if( found == null )
						{
							Services.Add( svc );
						}
                        else
                        {
                            found.Merge( svc );
						}
					}
				}

				if( other.Apps != null )
				{
					foreach( var app in other.Apps )
					{
                        var found = Apps.Find( (x) => x.Name == app.Name );
						if( found == null )
						{
							Apps.Add( app );
						}
                        else
						{
							found.Merge( app );
						}
					}
				}
			}
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
            public bool? ShowInPortFwdMode;
            public bool? ShowInLocalMode;
            public string IconFile;
            public string ExeFullPath;
            public string CmdLineArgs;
            public string StartupDir;
            //public bool? AdoptIfAlreadyRunning;
            //public EWindowStyle? WindowStyle = EWindowStyle.NotSet;
            public Dictionary<string, string> EnvVars = new Dictionary<string, string>();
            public Dictionary<string, string> LocalVars = new Dictionary<string, string>();
            //public string EnvVarPathToPrepend;
            //public string EnvVarPathToAppend;
            //public string PriorityClass; // idle, belownormal, normal, abovenormal, high, realtime; empty = normal
            //public bool? KillTree;
            public bool? UseShellExecute;

            public void Merge( App other )
			{
				if( other.Name != null ) Name = other.Name;
                if( other.Service != null ) Service = other.Service;
                if( other.ShowInLocalMode != null ) ShowInLocalMode = other.ShowInLocalMode;
				if( other.ShowInPortFwdMode != null ) ShowInPortFwdMode = other.ShowInPortFwdMode;
				if( other.IconFile != null ) IconFile = other.IconFile;
				if( other.ExeFullPath != null ) ExeFullPath = other.ExeFullPath;
				if( other.CmdLineArgs != null ) CmdLineArgs = other.CmdLineArgs;
				if( other.StartupDir != null ) StartupDir = other.StartupDir;

				if( other.EnvVars != null )
				{
					foreach( var kv in other.EnvVars )
					{
						EnvVars[kv.Key] = kv.Value;
					}
				}

				if( other.LocalVars != null )
				{
					foreach( var kv in other.LocalVars )
					{
						LocalVars[kv.Key] = kv.Value;
					}
				}

				if( other.UseShellExecute != null ) UseShellExecute = other.UseShellExecute;
			}
        }

    }
}
