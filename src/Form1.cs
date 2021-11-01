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
		const int gridColName = 0;
		const int gridColVNC = 1;
		const int gridColRDP = 2;
		const int gridColSSH = 3;
		const int gridColSCP = 4;
		const int gridColMax = gridColSCP;

        public Config.Main _cfg;
        List<Computer> Computers = new List<Computer>();

        public void LoadFromFile( string fileName )
        {
            _cfg = JsonConvert.DeserializeObject<Config.Main>( System.IO.File.ReadAllText( fileName ) );
            foreach( var c in _cfg.Computers )
            {
                var items = new object[gridColMax+1];
                
                items[gridColName] = $"{c.Label}";

                var svcs = c.Services;
                if( svcs == null ) continue;

                var comp = new Computer() { Cfg = c };
                AddService( comp, items, svcs.VNC != null, gridColVNC, Services.VNC );
                AddService( comp, items, svcs.RDP != null, gridColRDP, Services.RDP );
                AddService( comp, items, svcs.SSH != null, gridColSSH, Services.SSH );
                AddService( comp, items, svcs.SCP != null, gridColSCP, Services.SCP );

                var gridRowIdx = grdComputers.Rows.Add( items );

                comp.GridRowIdx = gridRowIdx;
                Computers.Add( comp );
            }

        }

        void AddService( Computer comp, object[] gridColItems, bool isConfigured, int gridColIndex, SvcDef svcDef )
        {
            if( isConfigured )
            {
                gridColItems[gridColIndex] = ResizeImage( new Bitmap( svcDef.Image ), new Size( 20, 20 ) );
                comp.Services.Add( new Service() { SvcDef = svcDef, GridColIndex=gridColIndex } );
            }
            else
            {
                gridColItems[gridColIndex] = ResizeImage( new Bitmap( Resource1.Empty ), new Size( 20, 20 ) );
            }

        }


        public frmMain()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadFromFile("config.json");
        }

		static Bitmap ResizeImage( Bitmap imgToResize, Size size )
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
                        var svc = (from x in comp.Services where x.GridColIndex == currentCol select x).FirstOrDefault();
                        if( svc != null )
                        {
                            // handle the click
                            OnStartServiceClicked( comp, svc );
                        }
                    }

				}
			}

        }

        void OnStartServiceClicked( Computer comp, Service svc )
        {
            MessageBox.Show($"Clicked {comp.IP} {svc.Name}");

            if( svc.Launcher == null || !svc.Launcher.Running )
            {
                svc.Launcher = svc.SvcDef.LauncherBuilder( comp );
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

        void KillServices()
        {
            foreach( var c in Computers )
            {
                foreach( var s in c.Services )
                {
                    if( s.Launcher != null )
                    {
                        s.Launcher.Kill();
                    }
                }
            }
            // give time for services to dies
            Thread.Sleep(1000);
        }

		private void frmMain_FormClosed( object sender, FormClosedEventArgs e )
		{
            KillServices();
		}
	}

}
