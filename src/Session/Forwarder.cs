using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoter
{
	public class Forwarder
	{
		Launcher Launcher;
		GlobalContext Ctx => GlobalContext.Instance;
		Session _session;

		public Forwarder( Session session )
		{
			_session = session;
			var app = Applications.ByName("plink");
			if( app == null ) return;
			var appDef = (AppDef)app.AppDef.Clone();
			appDef.CmdLineArgs = BuildPlinkArgs( session );
			Launcher = new Launcher( appDef, null, new Dictionary<string, string>() );
		}

		public void Dispose()
		{
			Launcher?.Kill();
			Launcher?.Dispose();
		}

		string BuildPlinkArgs( Session session )
		{
			var sb = new StringBuilder();
			// 
			//&plink.exe 10.0.103.7 -l student -pw Zaq1Xsw2 -P 22 -no-antispoof `
			sb.Append( $"{session.Conf.Gateway} -l {session.Conf.UserName} -pw {session.Conf.Password} -P 22 -no-antispoof ");
			foreach( var comp in _session.Computers )
			{
				if( comp.Conf.BehindGateway )
				{
					foreach( var svc in comp.Services )
					{
						// -L 7101:192.168.0.101:5900 
						var fwdArg = $"-L {svc.Port}:{comp.IP}:{svc.Conf.Port} ";
						sb.Append( fwdArg );
					}
				}
			}

			return sb.ToString();
		}

		
		public bool IsRunning => Launcher != null && Launcher.Running;
		
		public void Start()
		{
			if( Launcher == null ) return;

            if( !Launcher.Running )
            {
                Launcher.Launch();
            }

            if( Launcher.Running )
            {
                Launcher.MoveToForeground();
            }
		}

		public void Stop()
		{
			if( Launcher == null ) return;
			Launcher.Kill();
		}

	}
}
