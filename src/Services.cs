using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Remoter
{
    public class SvcDef
    {
        public string Name; // service id
        public Image Image; // grid icon
        public AppDef AppDef; // app to run when service is chosen for starting
        public Func<Computer, Launcher> LauncherBuilder; // Called before launching. Can prepare necessary config files etc.
    }

    public static class Services
	{
        //static Dictionary<string, SvcDef> ServiceByName = new Dictionary<string, SvcDef>();
        static public SvcDef VNC;
        static public SvcDef RDP;
        static public SvcDef SSH;
        static public SvcDef SCP;

        static Launcher DefaultLauncherBuilder( Computer comp, SvcDef svcDef )
        {
            var vars = new Dictionary<string,string>();
            vars["PORT"] = comp.Cfg.Services.ByName( svcDef.Name ).Port.ToString();
            return new Launcher( svcDef.AppDef, null, vars );
        }


        static Services()
        {
            VNC = new SvcDef()
            {
                Name = "VNC",
                Image = Resource1.VNC,
                AppDef = new AppDef()
                {
                    ExeFullPath = "tvnviewer.exe",
                    CmdLineArgs = "127.0.0.1:%PORT%",
                    StartupDir = ""
                }
            };
            VNC.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, VNC ); };
            
            RDP = new SvcDef()
            {
                Name = "RDP",
                Image = Resource1.RDP,
                AppDef = new AppDef()
                {
                    ExeFullPath = @"%windir%\system32\mstsc.exe",
                    CmdLineArgs = "/v:127.0.0.1:%PORT%",
                    StartupDir = ""
                }
            };
            RDP.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, RDP ); };

            SSH = new SvcDef()
            {
                Name = "SSH",
                Image = Resource1.SSH,
                AppDef = new AppDef()
                {
                    ExeFullPath = "ssh.exe",
                    CmdLineArgs = "127.0.0.1:%PORT%",
                    StartupDir = ""
                }
            };
            SSH.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, SSH ); };

            SCP = new SvcDef()
            {
                Name = "SCP",
                Image = Resource1.SCP,
                AppDef = new AppDef()
                {
                    ExeFullPath = "scp.exe",
                    CmdLineArgs = "127.0.0.1:%PORT%",
                    StartupDir = ""
                }
            };
            SCP.LauncherBuilder = (Computer comp) => { return DefaultLauncherBuilder( comp, SCP ); };

            //ServiceByName[VNC.Name] = VNC;
            //ServiceByName[RDP.Name] = RDP;
            //ServiceByName[SSH.Name] = SSH;
            //ServiceByName[SCP.Name] = SCP;
        }

        //public static SvcDef GetByName(string name)
        //{
        //    if( ServiceByName.TryGetValue( name, out var svcdef ) )
        //    {
        //        return svcdef;
        //    }
        //    return null;
        //}

	}

}
