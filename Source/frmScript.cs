using System;
using System.Windows.Forms;
using Engine;

namespace Sandbox
{
    public partial class frmScript : Form
    {
        public string Script
        {
            get { return script.Text; }
            set { script.Text = value; }
        }

        private string language;

        private string tmpScript;
        private bool btnOKPressed;

        public frmScript()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOKPressed = true;
            Close();
        }

        private void frmScript_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!btnOKPressed)
            {
                Script = tmpScript;
            }
        }

        private void frmScript_Load(object sender, EventArgs e)
        {
            var settings = Helpers.LoadSettings();
            language = settings.ScriptLanguage.ToString().ToLower();
            script.ConfigurationManager.Language = language;
            script.ConfigurationManager.Configure();

            tmpScript = Script;
            btnOKPressed = false;
        }
    }
}
