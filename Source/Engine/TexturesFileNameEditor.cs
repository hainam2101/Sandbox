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
            string path = Helpers.PATH_TEXTURES;

            if (!String.IsNullOrEmpty(path))
            {
                if(Directory.Exists(Path.Combine(Application.StartupPath, path)))
                {
                    openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, path);
                }
            }

            base.InitializeDialog(openFileDialog);
        }
    }
}



