namespace Remoter
{
	// 
	/// <summary>
	/// App using the forwarded port of some network service, for example VNC viewer
	/// Launched when clicking on the grid
	/// </summary>
	public class ConsumerApp
    {
        public string Name => App.Name;
        public Config.Service ServiceConf; // what service are we associated with (null = none)

        // app to be started when we click the icon in the grid
        public GridApp App;
        public Launcher Launcher; // non-nul if app already running

        public int GridColIndex = -1;  // where in the grid are we showing our icon (-1 = nowhere)

    }


}
