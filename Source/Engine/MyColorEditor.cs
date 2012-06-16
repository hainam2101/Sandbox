using System;                       // IServiceProvider
using System.ComponentModel;        // ITypeDescriptorContext
using System.Drawing;               // Color
using System.Drawing.Design;        // UITypeEditor, UITypeEditorEditStyle
using System.Windows.Forms;         // ColorDialog
using System.Windows.Forms.Design;  // IWindowsFormsEditorService

namespace Engine
{
    public class MyColorEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs pe)
        {
            try
            {
                Color c = Color.FromArgb(((MyColor)pe.Value).R, ((MyColor)pe.Value).G, ((MyColor)pe.Value).B);
                Brush br = new SolidBrush(c);
                pe.Graphics.FillRectangle(br, pe.Bounds);
            }
            catch { }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //// This tells it to show the [...] button which is clickable firing off EditValue below.
            //return UITypeEditorEditStyle.Modal;
            if (context != null) return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null)
            {
                // This is the code you want to run when the [...] is clicked and after it has been verified.

                // Get our currently selected color.
                MyColor color = (MyColor)value;

                // Create a new instance of the ColorDialog.
                ColorDialog selectionControl = new ColorDialog();

                // Set the selected color in the dialog.
                selectionControl.Color = Color.FromArgb(color.GetARGB());

                // Show the dialog.
                selectionControl.ShowDialog();

                // Return the newly selected color.
                value = new MyColor(selectionControl.Color.ToArgb());
                //value = ColorTranslator.ToHtml(selectionControl.Color);
            }

            return value;
        }
    }
}
