using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace Remoter
{
	public class Tools
	{
		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().Location;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		/// <summary>
		/// Replaces %ENVVAR% in a string with actual value of evn vars; undefined ones will be replaced with empty string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ExpandEnvVars(String str, bool leaveUnknown=false)
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(%\w+%)");

			while( match.Success )
			{
				string varName = match.Value.Replace("%", "").Trim();
				string varValue = Environment.GetEnvironmentVariable(varName);
				
				bool replace = true;
				
				if( varValue == null )
				{
					if( leaveUnknown )	// do not replace, leave as is
					{
						replace = false;
					}
					else // replace the unknown var with empty string
					{
						varValue = String.Empty; 
					}
				}

				if( replace )
				{
					str = str.Replace( match.Value, varValue );
				}
				match = match.NextMatch();
			}
			return str;
		}

		/// <summary>
		/// Replaces %VARNAME% in a string with actual value of the variable from given disctionary; undefined ones will be replaced with empty string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ExpandInternalVars(String str, Dictionary<string, string> variables, bool leaveUnknown=false)
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(%\w+%)");

			while( match.Success )
			{
				string varName = match.Value.Replace("%", "").Trim();
				string varValue;
				bool replace = true;
				if( !variables.TryGetValue( varName, out varValue ) )
				{
					if( leaveUnknown )	// do not replace, leave as is
					{
						replace = false;
					}
					else // replace the unknown var with empty string
					{
						varValue = String.Empty; 
					}
				}

				if( replace )
				{
					str = str.Replace( match.Value, varValue );
				}

				match = match.NextMatch();
			}
			return str;
		}

		///// <summary>
		///// Replaces %1  %2 etc. in a string with actual value from given array
		///// </summary>
		//public static string ExpandNumericVars(String str, List<string> parameters)
		//{
		//
		//	System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(%\d+)");
		//
		//	while( match.Success )
		//	{
		//		string varName = match.Value.Replace("%", "").Trim();
		//		int varIndex = -1;
		//		try{
		//		  varIndex = Convert.ToInt32(varName);
		//		}
		//		catch
		//		{
		//		}
		//		
		//		string varValue = String.Empty;
		//		if( varIndex >=0 && varIndex < parameters.Count )
		//		{
		//			varValue = parameters[varIndex];
		//		}
		//
		//		str = str.Replace( match.Value, varValue );
		//		match = match.NextMatch();
		//	}
		//	return str;
		//}

		/// <summary>
		/// Replaces any %VARNAME% with an ampty string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RemoveVars( String str )
		{

			System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(%\w+%)");

			while( match.Success )
			{
				string varName = match.Value.Replace("%", "").Trim();
				string varValue = String.Empty;
				str = str.Replace( match.Value, varValue );
				match = match.NextMatch();
			}
			return str;
		}

		// parses a list of strings in format key=value into a dictionary
		public static Dictionary<string, string> ParseKeyValList( IList<string> args )
		{
			var res = new Dictionary<string, string>();
			foreach( var a in args )
			{
				var arr = a.Split( new char[] {'='}, 2 );
				if( arr.Length == 2 )
				{
					res.Add( arr[0].Trim(), arr[1].Trim() );
				}
			}
			return res;
		}

		public static bool TryGetValueIgnoreKeyCase( Dictionary<string, string> keyValArgs, string key, out string value )
		{
			foreach( var kv in keyValArgs )
			{
				if( string.Compare( kv.Key, key, true ) == 0 )
				{
					value = kv.Value;
					return true;
				}
			}
			value = string.Empty;
			return false;
		}

		public static bool GetEnumValueByNameIgnoreCase<T>( string name, out T value ) where T:IComparable
		{
			int i=0;
			foreach( var eName in Enum.GetNames( typeof(T) ) )
			{
				if( string.Compare( eName, name, true ) == 0 )
				{
					// strange way how to get enum value :-(
					var enumValues = Enum.GetValues(typeof(T)).Cast<T>();
					int j=0;
					T y = default(T);
					foreach ( T x in enumValues )
					{
						y = x;
						if( j==i ) break;
					}
					value = y;
					return true;
				}
				i++;
			}
			value = default(T);
			return false;
		}

        public static string GetExePath()
        {
            var assemblyExe = System.Reflection.Assembly.GetEntryAssembly().Location;
            if (assemblyExe.StartsWith("file:///")) assemblyExe = assemblyExe.Remove(0, 8);
			return assemblyExe;
        }

        public static string GetExeDir()
        {
			return System.IO.Path.GetDirectoryName( GetExePath() );
		}

		public static Bitmap ResizeImage( Bitmap imgToResize, Size size )
		{
			try
			{
				Bitmap b = new Bitmap( size.Width, size.Height );
				using( Graphics g = Graphics.FromImage( b ) )
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage( imgToResize, 0, 0, size.Width, size.Height );
				}
				return b;
			}
			catch { }
			return null;
		}

}

}
