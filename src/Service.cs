namespace Remoter
{
	public class Service
    {
        public int GridColIndex;
        public SvcDef SvcDef;
        public Launcher Launcher; // non-nul if service already running
        public string Name => SvcDef.Name;
    }

}
