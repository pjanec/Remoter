
namespace Remoter
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnLoad = new System.Windows.Forms.Button();
			this.grdComputers = new System.Windows.Forms.DataGridView();
			this.CompName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnStart = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grdComputers)).BeginInit();
			this.SuspendLayout();
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(12, 8);
			this.btnLoad.Margin = new System.Windows.Forms.Padding(5);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(114, 43);
			this.btnLoad.TabIndex = 7;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// grdComputers
			// 
			this.grdComputers.AllowUserToAddRows = false;
			this.grdComputers.AllowUserToDeleteRows = false;
			this.grdComputers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.grdComputers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdComputers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CompName});
			this.grdComputers.Location = new System.Drawing.Point(12, 60);
			this.grdComputers.Margin = new System.Windows.Forms.Padding(5);
			this.grdComputers.Name = "grdComputers";
			this.grdComputers.RowHeadersVisible = false;
			this.grdComputers.RowHeadersWidth = 72;
			this.grdComputers.Size = new System.Drawing.Size(524, 240);
			this.grdComputers.TabIndex = 9;
			this.grdComputers.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdComputers_CellEnter);
			this.grdComputers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdComputers_CellFormatting);
			this.grdComputers.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdComputers_CurrentCellDirtyStateChanged);
			this.grdComputers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdComputers_MouseClick);
			// 
			// CompName
			// 
			this.CompName.FillWeight = 33F;
			this.CompName.HeaderText = "Computer";
			this.CompName.MinimumWidth = 9;
			this.CompName.Name = "CompName";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(422, 7);
			this.btnStart.Margin = new System.Windows.Forms.Padding(5);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(114, 43);
			this.btnStart.TabIndex = 10;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 325);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.grdComputers);
			this.Controls.Add(this.btnLoad);
			this.Margin = new System.Windows.Forms.Padding(5);
			this.Name = "frmMain";
			this.Text = "Remoter";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.grdComputers)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView grdComputers;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.DataGridViewTextBoxColumn CompName;
	}
}

