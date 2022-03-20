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


        GlobalContext Context = new GlobalContext();

        public GlobalContext Ctx => GlobalContext.Instance;
        public Session Session;
        public List<Computer> Computers => Session.Computers;

        public void InitSession( string fileName )
        {
            Session = new Session( fileName );

            // define columns
            foreach( var app in Applications.Apps )
            {
                var col = new System.Windows.Forms.DataGridViewImageColumn();
			    col.FillWeight = 15F;
			    col.MinimumWidth = 9;
			    col.Name = app.Name;
			    col.HeaderText = app.Name;
                grdComputers.Columns.Add( col );
            }

            // on column per service app
            foreach( var comp in Computers )
            {
                var items = new object[grdComputers.Columns.Count];
                
                items[gridColName] = $"{comp.Conf.Label}";

                int gridCol = gridColName+1;
                foreach( var app in Applications.Apps )
                {
                    var consumer = comp.Apps.Find( (x) => app.Name == x.Name );    
                    if( consumer != null ) // is app configured for this computer?
                    {
                        items[gridCol] = Tools.ResizeImage( new Bitmap( app.Image ), new Size( 20, 20 ) );
                        consumer.GridColIndex = gridCol;
                    }
                    else
                    {
                        items[gridCol] = Tools.ResizeImage( new Bitmap( Resource1.Empty ), new Size( 20, 20 ) );
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
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitSession("session.json");
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

        void OnStartServiceClicked( Computer comp, ConsumerApp svc )
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

		private void frmMain_FormClosed( object sender, FormClosedEventArgs e )
		{
            Session?.Dispose();
		}

		private void btnStart_Click( object sender, EventArgs e )
		{
            Session?.Start();
		}
	}

}
