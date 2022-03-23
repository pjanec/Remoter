namespace Remoter
{
	// 
	/// <summary>
	/// App using the forwarded port of some network service, for example VNC viewer
	/// Launched when clicking on the grid
	/// </summary>
	public class GridApp
    {
        public string Name => App.Name;
        public Service Service;
        public bool ShowInPortFwdMode => App.ShowInPortFwdMode;
        public bool ShowInLocalMode => App.ShowInLocalMode;

        // app to be started when we click the icon in the grid
        public App App;
        public Launcher Launcher; // non-nul if app already running

        public int GridColIndex = -1;  // where in the grid are we showing our icon (-1 = nowhere)

    }


}
