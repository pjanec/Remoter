using System.Collections.Generic;

namespace Remoter
{
    public enum EWindowStyle
    {
        NotSet,
        Normal,
        Minimized,
        Maximized,
        Hidden
    }

	public class AppDef
    {
        public string ExeFullPath;
        public string CmdLineArgs;
        public string StartupDir;
        public bool AdoptIfAlreadyRunning;
        public EWindowStyle WindowStyle = EWindowStyle.NotSet;
        public Dictionary<string, string> EnvVarsToSet = new Dictionary<string, string>();
        public Dictionary<string, string> LocalVarsToSet = new Dictionary<string, string>();
        public string EnvVarPathToPrepend;
        public string EnvVarPathToAppend;
        public string PriorityClass; // idle, belownormal, normal, abovenormal, high, realtime; empty = normal
        public bool KillTree;
    }

}