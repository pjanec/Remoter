using System;
using System.Collections.Generic;
using System.Drawing;

namespace Remoter
{
	/// <summary>
	/// Application that can be started by clicking in the grid
	/// </summary>
	public class App : ICloneable
    {
        public string Name; 
        public string Service; // service id
        public bool ShowInPortFwdMode = true;
        public bool ShowInLocalMode = true;
        public Image Image; // grid icon
        public AppDef AppDef = new AppDef(); // app to run when service is chosen for starting
        public Func<Session, Computer, Launcher> LauncherBuilder; // Called before launching. Can prepare necessary config files etc.


        public object Clone()
        {
            var app = (App)MemberwiseClone();
            app.AppDef = (AppDef) AppDef.Clone();
            return app;
        }

        public void LoadFromConfig( Config.App def )
        {
            if( def.Name != null ) Name = def.Name;
            if( def.Service != null ) Service = def.Service;
            
            if( def.ExeFullPath != null ) AppDef.ExeFullPath = def.ExeFullPath;
            if( def.CmdLineArgs != null ) AppDef.CmdLineArgs = def.CmdLineArgs;
            if( def.StartupDir != null )  AppDef.StartupDir = def.StartupDir;
            if( def.UseShellExecute != null ) AppDef.UseShellExecute = def.UseShellExecute.Value;
            if( def.ShowInLocalMode != null ) ShowInLocalMode = def.ShowInLocalMode.Value;
            if( def.ShowInPortFwdMode != null ) ShowInPortFwdMode = def.ShowInPortFwdMode.Value;
                
            if( def.IconFile != null )
            {
                try   { Image = new Bitmap( def.IconFile ); }
                catch { Image = Resource1.Unknown; }
            }

            if( def.EnvVars != null )
            {
                // add/replace
                foreach( var kv in def.EnvVars )
                {
                    AppDef.EnvVarsToSet[kv.Key] = kv.Value;    
                }
            }

            if( def.LocalVars != null )
            {
                // add/replace
                foreach( var kv in def.LocalVars )
                {
                    AppDef.LocalVarsToSet[kv.Key] = kv.Value;    
                }
            }

            LauncherBuilder = (Session session, Computer comp) => { return DefaultLauncherBuilder( session, comp, this ); };
        }

        static Launcher DefaultLauncherBuilder( Session session, Computer comp, App app )
        {
            var vars = new Dictionary<string,string>();
            var svc = comp.Services.Find( (x) => x.Name == app.Service );
            if( svc != null )
            {
                vars["SVC_IP"] = svc.IP;
                vars["SVC_PORT"] = svc.Port.ToString();
                vars["SVC_USERNAME"] = svc.UserName;
                vars["SVC_PASSWORD"] = svc.Password;
                vars["GW_IP"] = session.IsPortForwarding ? session.Gateway.ExternalIP : session.Gateway.InternalIP;
                vars["GW_PORT"] = session.Gateway.Port.ToString();
                vars["GW_USERNAME"] = session.Gateway.UserName;
                vars["GW_PASSWORD"] = session.Gateway.Password;
            }
            vars["APP_NEW_GUID"] = Guid.NewGuid().ToString();
            return new Launcher( app.AppDef, Tools.AssemblyDirectory, vars );
        }

    }

}
