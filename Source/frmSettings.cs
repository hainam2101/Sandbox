using System;
using System.Windows.Forms;
using Engine;

namespace Sandbox
{
    public partial class frmSettings : Form
    {
        private ProgramSettings settings;
        private ProgramSettings tmpSettings;
        private bool OKPressed;
        public frmSettings(ProgramSettings settings)
        {
            this.settings = settings;
            tmpSettings = settings;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = settings;
            propertyGrid.Update();
        }

        private void frmSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid.Update();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OKPressed = true;
            Helpers.SaveSettings(settings, ProductName);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!OKPressed)
            {
                settings = tmpSettings;
            }
        }
    }
}