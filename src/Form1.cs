using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Remoter
{
	public partial class frmMain : Form
    {
		const int gridColCompName = 0;


        GlobalContext Context = new GlobalContext();

        public GlobalContext Ctx => GlobalContext.Instance;
        public Session Session;
        public List<Computer> Computers => Session.Computers;

        public void InitSession( string fileName )
        {
            if( Session != null ) Session.Dispose();
            Session = new Session( fileName );
            ReloadFromSession();
        }

        static int AppIconWidth = 20;

        public void ReloadFromSession()
        {
            Text = $"Remoter - {Session.Name}";

            // computer name column
            grdComputers.Columns.Clear();
            {
                var col = new System.Windows.Forms.DataGridViewTextBoxColumn();
			    col.FillWeight = 50F;
			    col.MinimumWidth = 100;
			    col.Name = $"ComputerName";
			    col.HeaderText = "Computer";
                grdComputers.Columns.Add( col );
            }

            // find the number of comulns needed - the highest amount of apps per computer
            int maxCols = 0;
            foreach( var comp in Computers )
            {
                if( comp.Apps.Count > maxCols )
                    maxCols = comp.Apps.Count;
            }

            for( int i=0; i < maxCols; i++ )
            {
                var col = new System.Windows.Forms.DataGridViewImageColumn();
			    col.FillWeight = 10F;
			    col.MinimumWidth = AppIconWidth;
			    col.Name = $"Col{i+1}";
			    col.HeaderText = "";
                grdComputers.Columns.Add( col );
            }

            // on column per service app
            foreach( var comp in Computers )
            {
                var items = new object[grdComputers.Columns.Count];
                
                items[gridColCompName] = $"{comp.Conf.Label}";

                int gridCol = gridColCompName+1;
                for( int i=0; i < maxCols; i++ )
                {
                    GridApp app = i < comp.Apps.Count ? comp.Apps[i] : null;

                    if( app != null ) // is app configured for this computer?
                    {
                        bool activateApp =
                            (Session.IsPortForwarding && app.ShowInPortFwdMode)
                              ||
                            (!Session.IsPortForwarding && app.ShowInLocalMode);
                    

                        if( activateApp )
                        {
                            items[gridCol] = Tools.ResizeImage( new Bitmap(app.App.Image), new Size( AppIconWidth, AppIconWidth ) );
                            app.GridColIndex = gridCol;
                        }
                        else
                        {
                            items[gridCol] = Tools.ResizeImage( new Bitmap( Resource1.Empty ), new Size( AppIconWidth, AppIconWidth ) );
                            app.GridColIndex = -1; // do not respond to clicks
                        }
                    }
                    else
                    {
                        items[gridCol] = Tools.ResizeImage( new Bitmap( Resource1.Empty ), new Size( AppIconWidth, AppIconWidth ) );
                    }

                    gridCol++;
                }

                var gridRowIdx = grdComputers.Rows.Add( items );

                comp.GridRowIdx = gridRowIdx;
            }

        }
    
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var fn = Tools.PickFileToOpen(
                "Open session file",
				"session files (*.json)|*.json",
                "" );
            if( !string.IsNullOrEmpty( fn ) )
            {
                InitSession(fn);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string defaultSessionFile = "session.json";

            // take session file from the first cmd line argument
            string[] args = Environment.GetCommandLineArgs();
            if( args.Length > 1 )
                defaultSessionFile = args[1];

            // load it if it exists
            if( System.IO.File.Exists( defaultSessionFile ) )
            {
                InitSession( defaultSessionFile );
            }

            UpdateForwardingStatus();
        }

        private void grdComputers_MouseClick(object sender, MouseEventArgs e)
        {
			var hti = grdComputers.HitTest( e.X, e.Y );
			int currentRow = hti.RowIndex;
			int currentCol = hti.ColumnIndex;

			if( currentRow >= 0 ) // ignore header clicks
			{
				DataGridViewRow focused = grdComputers.Rows[currentRow];

				if( e.Button == MouseButtons.Left )
				{
                    // find computer and the service clicked
                    var comp = (from x in Computers where x.GridRowIdx==currentRow select x).FirstOrDefault();
                    if( comp != null )
                    {
                        var svc = (from x in comp.Apps where x.GridColIndex == currentCol select x).FirstOrDefault();
                        if( svc != null )
                        {
                            // handle the click
                            OnStartServiceClicked( comp, svc );
                        }
                    }

				}
			}

        }

        void OnStartServiceClicked( Computer comp, GridApp svc )
        {
            //MessageBox.Show($"Clicked {comp.IP} {svc.Name}");

            if( svc.Launcher == null || !svc.Launcher.Running )
            {
                svc.Launcher = svc.App.LauncherBuilder( Session, comp );
                try
                {
                    svc.Launcher.Launch();
                }
                catch( System.Exception ex )
                {
                    MessageBox.Show($"Failed to run {svc.Name}. Reason: {ex.Message}");
                    svc.Launcher = null;
                }
            }

            if( svc.Launcher != null && svc.Launcher.Running )
            {
                svc.Launcher.MoveToForeground();
            }

        }


        private void grdComputers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            var datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if(datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void grdComputers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var datagridview = sender as DataGridView;
            datagridview.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

		private void frmMain_FormClosed( object sender, FormClosedEventArgs e )
		{
            Session?.Dispose();
		}

		private void grdComputers_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
		{
            var comp = Computers[e.RowIndex];

            DataGridViewCell cell = this.grdComputers.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if( e.ColumnIndex == gridColCompName )
            {
                if( comp.IsRemote )
                {
                    cell.ToolTipText = $"{comp.IP} (remote)";
            }
                else
                {
                    cell.ToolTipText = $"{comp.IP} (local)";
                }
            }
            else
            {
                int appIndex = e.ColumnIndex-1;
                var app = appIndex < comp.Apps.Count ? comp.Apps[appIndex] : null;
                if( app != null )
                {
                    var text = app.Name;
                    var svc = app.Service;
                    if( svc != null )
                    {
                        if( comp.IsRemote )
                        {
                            text += $", {svc.Name} {svc.FwdIP}:{svc.FwdPort} => {svc.NativeIP}:{svc.NativePort}";
                        }
                        else
                        {
                            text += $", {svc.Name} {svc.NativeIP}:{svc.NativePort}";
                        }
                    }
                    else
                    {
                    }
                    cell.ToolTipText = text;

                }
			}
		}

		bool _wasForwarderRunning = false;


        void ReevaluateToolTips()
        {
            foreach( DataGridViewRow row in this.grdComputers.Rows )
            {
                foreach( DataGridViewCell cell in row.Cells )
                {
                    // force the tooltips to be recalculated
                    // https://stackoverflow.com/questions/10651702/is-there-a-way-to-force-a-datagridview-to-fire-its-cellformatting-event-for-all
                    var o = cell.FormattedValue;
                }
            }
        }

        void UpdateForwardingStatus()
		{
            btnStart.Enabled = Session != null;

            if( Session == null ) return;

            if( !_wasForwarderRunning && Session.IsPortForwarding )
            {
                btnStart.Text = "Stop Fwd";
                _wasForwarderRunning = Session.IsPortForwarding;
                ForwardingStatusChanged();
            }
            else
            if( _wasForwarderRunning && !Session.IsPortForwarding )
            {
                btnStart.Text = "Start Fwd";
                _wasForwarderRunning = Session.IsPortForwarding;
                ForwardingStatusChanged();
            }
		}

        void ForwardingStatusChanged()
        {
            ReevaluateToolTips();
            ReloadFromSession();
        }


		private void btnStart_Click( object sender, EventArgs e )
		{
            if( Session == null ) return;
            if( !Session.IsPortForwarding )
            {
                Session?.Start();
            }
            else
            {
                Session?.Stop();
            }
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
            UpdateForwardingStatus();
		}

	}

}
