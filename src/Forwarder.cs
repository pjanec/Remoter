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
		List<Computer> Computers;

		public Forwarder(List<Computer> computers)
		{
			Computers = computers;
			var app = Applications.GetByName("plink");
			if( app == null ) return;
			Launcher = new Launcher( app.AppDef, null, new Dictionary<string, string>() );
		}

		public void Dispose()
		{
			Launcher?.Kill();
			Launcher?.Dispose();
		}

		string BuildPlinkArgs()
		{
			var sb = new StringBuilder();
			// 
			//&plink.exe 10.0.103.7 -l student -pw Zaq1Xsw2 -P 22 -no-antispoof `
			//sb.Append( $"{Context.SessionConf.Gateway} -l {Context.SessionConf.UserName} -pw {Context.SessionConf.Password} -P 22 -no-antispoof ");
			foreach( var comp in Computers )
			{
				foreach( var app in comp.Services )
				{
					var svcCfg = app.ServiceConf;

					// -L 7101:192.168.0.101:5900 
					var fwdArg = $"-L {app.LocalPort}:{comp.IP}:{svcCfg.Port} ";
					sb.Append( fwdArg );

				}
			}

			return sb.ToString();
		}

		
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
