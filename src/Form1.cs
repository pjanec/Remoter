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


        //Context Context = new Context();

        public Config.Session _cfg;
        List<Computer> Computers = new List<Computer>();
        Forwarder Forwarder;
        int LocalPortBase;

        public void LoadSessionConfig( string fileName )
        {
            LocalPortBase = 40000;

            _cfg = JsonConvert.DeserializeObject<Config.Session>( System.IO.File.ReadAllText( fileName ) );

            // define columns
            foreach( var app in Applications.ServiceApps )
            {
                var col = new System.Windows.Forms.DataGridViewImageColumn();
			    col.FillWeight = 15F;
			    col.MinimumWidth = 9;
			    col.Name = app.Name;
			    col.HeaderText = app.Name;
                grdComputers.Columns.Add( col );
            }


            // fill columns for each computer in the session
            foreach( var c in _cfg.Computers )
            {
                var items = new object[grdComputers.Columns.Count];
                
                items[gridColName] = $"{c.Label}";

                var svcs = c.Services;
                if( svcs == null ) continue; // no services defined for a computer?

                var comp = new Computer() { Cfg = c };
                int gridCol = gridColName+1;
                foreach( var app in Applications.ServiceApps )
                {
                    AddService( comp, items, gridCol, app );
                    gridCol++;
                }

                var gridRowIdx = grdComputers.Rows.Add( items );

                comp.GridRowIdx = gridRowIdx;
                Computers.Add( comp );
            }

        }

    
        void AddService( Computer comp, object[] gridColItems, int gridColIndex, App app )
        {
            var serviceConf = comp.Cfg.Services.Find( (x) => x.Name == app.Name );

            bool isConfigured = serviceConf != null;

            if( isConfigured )
            {
                gridColItems[gridColIndex] = ResizeImage( new Bitmap( app.Image ), new Size( 20, 20 ) );

                comp.Services.Add(
                    new ServiceApp()
                    {
                        ServiceConf = serviceConf,
                        LocalPort = ++LocalPortBase,
                        App = app,
                        GridColIndex=gridColIndex
                    }
                );
            }
            else
            {
                gridColItems[gridColIndex] = ResizeImage( new Bitmap( Resource1.Empty ), new Size( 20, 20 ) );
            }
        }

        
        void StartForwarding()
        {
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
            LoadSessionConfig("session.json");
            Forwarder = new Forwarder( Computers );
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

        void OnStartServiceClicked( Computer comp, ServiceApp svc )
        {
            MessageBox.Show($"Clicked {comp.IP} {svc.Name}");

            if( svc.Launcher == null || !svc.Launcher.Running )
            {
                svc.Launcher = svc.App.LauncherBuilder( comp );
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
            Forwarder.Dispose();
		}

		private void btnStart_Click( object sender, EventArgs e )
		{
            Forwarder.Start();
		}
	}

}
