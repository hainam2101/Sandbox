using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using MTV3D65;

namespace Engine
{
    /// <summary>
    /// ObjectBase class.
    /// </summary>
    public abstract class ObjectBase : IObjectBase
    {
        protected ObjectBase(ICore core)
        {
            Core = core;
            Scale = new VECTOR3D(1.0f, 1.0f, 1.0f);
            Selected = false;
            Visible = true;
            Renderable = true;
            core.AllObjects.Add(this);
            custom = new CustomCollection();
            ScriptEnabled = core.Settings.ScriptEnabled;
            Script = core.Settings.Script;
        }

        protected ICore Core { get; set; }

        #region IObjectBase Members

        [Category("Base")]
        private string name;
        public virtual string Name { get { return name; } 
            set 
            {
                Script = Script.Replace("$Name", value);
                Script = Script.Replace("$name", value.ToLower());

                if (name != null)
                {
                    if (!name.Equals(value) && !Core.LoadingScene)
                    {
                        var result = MessageBox.Show(string.Format("Replace {0} to {1} in script?", name, value), "Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            Script = Script.Replace(name, value);
                        }
                    }
                }
                
                name = value;
            }
        }

        [Category("Base")]
        public virtual bool Visible { get; set; }

        [Browsable(false)]
        public string UniqueId { get; set; }

        /*[Description("Select a file..."), Category("Base"), Browsable(true),
        Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]*/

        [Category("Base"), ReadOnly(true)]
        public virtual string FileName { get; set; }

        [Category("Transform")]
        public virtual VECTOR3D Position { get; set; }

        [Category("Transform")]
        public virtual VECTOR3D Rotation { get; set; }

        [Category("Transform")]
        public virtual VECTOR3D Scale { get; set; }

        private CustomCollection custom;
        [TypeConverter(typeof(CustomCollectionConverter)), RefreshProperties(RefreshProperties.All)]
        [Category("Misc"), DisplayName("Custom parameters")]
        [Description("Custom parameters collection.")]
        public CustomCollection CustomParams { get { return custom; } }

        public void SetCustomCollection(CustomCollection custColl)
        {
            for (var i = 0; i < custColl.Count; i++ )
            {
                custom.Add(new Custom(custColl[i].Name, custColl[i].Value));
            }
        }

        [Category("Script"), DisplayName("Enabled")]
        [Description("Defines if script is enabled.")]
        public bool ScriptEnabled { get; set; }

        [Category("Script"), DisplayName("Script")]
        [Description("Custom script.")]
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
        public string Script { get; set; }

        [Browsable(false)]
        public bool Selected { get; set; }

        [Browsable(false)]
        public bool Renderable { get; set; }

        public abstract void Update();
        public abstract void Select();
        public abstract void Deselect();
        public abstract void GetBoundingBox(ref TV_3DVECTOR min, ref TV_3DVECTOR max);
        public abstract TVMesh GetMesh();

        #endregion
    }
}