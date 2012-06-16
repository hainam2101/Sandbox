namespace Sandbox
{
    partial class frmScript
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.script = new ScintillaNet.Scintilla();
            this.btnOK = new System.Windows.Forms.Button();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.script)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.script);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnOK);
            this.splitContainer.Size = new System.Drawing.Size(809, 558);
            this.splitContainer.SplitterDistance = 505;
            this.splitContainer.TabIndex = 0;
            // 
            // script
            // 
            this.script.Dock = System.Windows.Forms.DockStyle.Fill;
            this.script.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.script.LineWrap.VisualFlags = ScintillaNet.WrapVisualFlag.End;
            this.script.Location = new System.Drawing.Point(0, 0);
            this.script.Margins.Margin1.AutoToggleMarkerNumber = 0;
            this.script.Margins.Margin1.IsClickable = true;
            this.script.Margins.Margin2.Width = 16;
            this.script.Name = "script";
            this.script.Size = new System.Drawing.Size(809, 505);
            this.script.Styles.BraceBad.Size = 9F;
            this.script.Styles.BraceLight.Size = 9F;
            this.script.Styles.ControlChar.Size = 9F;
            this.script.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.script.Styles.Default.Size = 9F;
            this.script.Styles.IndentGuide.Size = 9F;
            this.script.Styles.LastPredefined.Size = 9F;
            this.script.Styles.LineNumber.Size = 9F;
            this.script.Styles.Max.Size = 9F;
            this.script.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(722, 14);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 558);
            this.Controls.Add(this.splitContainer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScript";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script";
            this.Load += new System.EventHandler(this.frmScript_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScript_FormClosed);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.script)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnOK;
        private ScintillaNet.Scintilla script;


    }
}