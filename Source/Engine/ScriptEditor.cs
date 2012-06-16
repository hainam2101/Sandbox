using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Sandbox;

namespace Engine
{
    public class ScriptEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            
            if (svc != null)
            {
                frmScript form = new frmScript();
                form.Script = value as string;
                svc.ShowDialog(form);
                value = form.Script;
            }

            return value;
        }
    }
}