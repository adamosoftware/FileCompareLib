namespace AdamOneilSoftware
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.fvEdited = new AdamOneilSoftware.FileViewer();
			this.fvTarget = new AdamOneilSoftware.FileViewer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.fvEdited);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.fvTarget);
			this.splitContainer1.Size = new System.Drawing.Size(624, 403);
			this.splitContainer1.SplitterDistance = 306;
			this.splitContainer1.TabIndex = 0;
			// 
			// fvEdited
			// 
			this.fvEdited.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fvEdited.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fvEdited.Location = new System.Drawing.Point(0, 0);
			this.fvEdited.Name = "fvEdited";
			this.fvEdited.OppositeViewer = this.fvTarget;
			this.fvEdited.Size = new System.Drawing.Size(306, 403);
			this.fvEdited.TabIndex = 0;
			this.fvEdited.FileSelected += new System.EventHandler(this.fvEdited_FileSelected);
			// 
			// fvTarget
			// 
			this.fvTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fvTarget.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fvTarget.Location = new System.Drawing.Point(0, 0);
			this.fvTarget.Name = "fvTarget";
			this.fvTarget.OppositeViewer = this.fvEdited;
			this.fvTarget.Size = new System.Drawing.Size(314, 403);
			this.fvTarget.TabIndex = 0;
			this.fvTarget.FileSelected += new System.EventHandler(this.fvTarget_FileSelected);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 403);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "File Compare";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private AdamOneilSoftware.FileViewer fvEdited;
		private AdamOneilSoftware.FileViewer fvTarget;
	}
}

