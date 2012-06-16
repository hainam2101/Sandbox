using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;

namespace Sandbox
{
    public partial class frmAbout : Form
    {
        string emMail = "tadas.zulonas@gmail.com";

        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblProgramName.Text = string.Format("{0} {1}", Helpers.PROGRAM_NAME, Helpers.PROGRAM_VERSION);
            lblInfo.Text = "By Tadas Zulonas";

            LinkLabel lnkMailTo = new LinkLabel();
            lnkMailTo.Parent = this;
            lnkMailTo.Text = emMail;
            lnkMailTo.Location = new Point(48, 69);
            lnkMailTo.AutoSize = true;
            lnkMailTo.TextAlign = ContentAlignment.TopCenter;
            lnkMailTo.BorderStyle = BorderStyle.None;
            lnkMailTo.Links.Add(0, lnkMailTo.Text.ToString().Length,
                String.Concat("mailto:", emMail));
            lnkMailTo.LinkClicked +=
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                    lnkMailTo_LinkClicked);
        }

        private void CloseForm(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void frmAbout_KeyDown(object sender, KeyEventArgs e)
        {
            CloseForm(e);
        }

        private void btnClose_KeyDown_1(object sender, KeyEventArgs e)
        {
            CloseForm(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lnkMailTo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(String.Concat("mailto:", emMail, "?subject=", Helpers.PROGRAM_NAME, " ", Helpers.PROGRAM_VERSION));
            }
            catch (Exception)
            {
                Clipboard.SetText(emMail);
                MessageBox.Show("An e-mail address was copied to clipboard.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
