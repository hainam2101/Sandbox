namespace Sandbox
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.lblProgramName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProgramName
            // 
            this.lblProgramName.Location = new System.Drawing.Point(12, 20);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(197, 23);
            this.lblProgramName.TabIndex = 0;
            this.lblProgramName.Text = "lblProgramName";
            this.lblProgramName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(134, 110);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnClose_KeyDown_1);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(12, 43);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(197, 23);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "lblInfo";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 145);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblProgramName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAbout_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProgramName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInfo;
    }
}