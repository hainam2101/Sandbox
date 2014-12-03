using System;
using System.IO;
using System.Windows.Forms;
using Engine;

namespace Sandbox
{
    public class TexturesFileNameEditor : System.Windows.Forms.Design.FileNameEditor
    {
        protected override void InitializeDialog(System.Windows.Forms.OpenFileDialog openFileDialog)
        {
            var settings = Helpers.LoadSettings();

            if (!String.IsNullOrEmpty(settings.FolderTextures))
            {
                if (Directory.Exists(Path.Combine(Application.StartupPath, settings.FolderTextures)))
                {
                    openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, settings.FolderTextures);
                }
            }

            base.InitializeDialog(openFileDialog);
        }
    }
}



