
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
			this.VNC = new System.Windows.Forms.DataGridViewImageColumn();
			this.RDP = new System.Windows.Forms.DataGridViewImageColumn();
			this.SSH = new System.Windows.Forms.DataGridViewImageColumn();
			this.SCP = new System.Windows.Forms.DataGridViewImageColumn();
			this.btnStart = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grdComputers)).BeginInit();
			this.SuspendLayout();
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(15, 10);
			this.btnLoad.Margin = new System.Windows.Forms.Padding(6);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(139, 52);
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
            this.CompName,
            this.VNC,
            this.RDP,
            this.SSH,
            this.SCP});
			this.grdComputers.Location = new System.Drawing.Point(15, 72);
			this.grdComputers.Margin = new System.Windows.Forms.Padding(6);
			this.grdComputers.Name = "grdComputers";
			this.grdComputers.RowHeadersVisible = false;
			this.grdComputers.RowHeadersWidth = 72;
			this.grdComputers.Size = new System.Drawing.Size(444, 288);
			this.grdComputers.TabIndex = 9;
			this.grdComputers.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdComputers_CellEnter);
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
			// VNC
			// 
			this.VNC.FillWeight = 15F;
			this.VNC.HeaderText = "VNC";
			this.VNC.MinimumWidth = 9;
			this.VNC.Name = "VNC";
			// 
			// RDP
			// 
			this.RDP.FillWeight = 15F;
			this.RDP.HeaderText = "RDP";
			this.RDP.MinimumWidth = 9;
			this.RDP.Name = "RDP";
			// 
			// SSH
			// 
			this.SSH.FillWeight = 15F;
			this.SSH.HeaderText = "SSH";
			this.SSH.MinimumWidth = 9;
			this.SSH.Name = "SSH";
			// 
			// SCP
			// 
			this.SCP.FillWeight = 15F;
			this.SCP.HeaderText = "SCP";
			this.SCP.MinimumWidth = 9;
			this.SCP.Name = "SCP";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(320, 10);
			this.btnStart.Margin = new System.Windows.Forms.Padding(6);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(139, 52);
			this.btnStart.TabIndex = 10;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 386);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.grdComputers);
			this.Controls.Add(this.btnLoad);
			this.Margin = new System.Windows.Forms.Padding(6);
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
		private System.Windows.Forms.DataGridViewTextBoxColumn CompName;
		private System.Windows.Forms.DataGridViewImageColumn VNC;
		private System.Windows.Forms.DataGridViewImageColumn RDP;
		private System.Windows.Forms.DataGridViewImageColumn SSH;
		private System.Windows.Forms.DataGridViewImageColumn SCP;
		private System.Windows.Forms.Button btnStart;
	}
}

