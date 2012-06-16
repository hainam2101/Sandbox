using System.ComponentModel;
using System.Windows.Forms;
using MTV3D65;
using Sandbox;
using Sandbox.Properties;

namespace Engine
{
    public class SkyBox : ObjectBase
    {
        private ICore core;
        private string front, back, left, right, top, bottom;

        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Front texture.")]
        public string Front { get { return Helpers.IsTextureFromMemory(front) ? string.Empty : front; } set { front = value; SetTextures(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Back texture.")]
        public string Back { get { return Helpers.IsTextureFromMemory(back) ? string.Empty : back; } set { back = value; SetTextures(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Left texture.")]
        public string Left { get { return Helpers.IsTextureFromMemory(left) ? string.Empty : left; } set { left = value; SetTextures(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Right texture.")]
        public string Right { get { return Helpers.IsTextureFromMemory(right) ? string.Empty : right; } set { right = value; SetTextures(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Top texture.")]
        public string Top { get { return Helpers.IsTextureFromMemory(top) ? string.Empty : top; } set { top = value; SetTextures(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Bottom texture.")]
        public string Bottom { get { return Helpers.IsTextureFromMemory(bottom) ? string.Empty : bottom; } set { bottom = value; SetTextures(); } }
        [Browsable(false)]
        public string FrontFileName, BackFileName, LeftFileName, RightFileName, TopFileName, BottomFileName;

        [Browsable(false)]
        public override VECTOR3D Position { get; set; }
        [Browsable(false)]
        public override VECTOR3D Rotation { get; set; }
        [Browsable(false)]
        public override VECTOR3D Scale { get; set; }
        [Browsable(false)]
        public override bool Visible { get; set; }
        [Browsable(false)]
        public override string FileName { get; set; }

        public SkyBox(ICore core)
            : base(core)
        {
            this.core = core;

            Front = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            Back = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            Left = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            Right = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            Top = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            Bottom = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);

            core.Atmosphere.SkyBox_Enable(true);
            core.Atmosphere.SkySphere_Enable(false);
            core.Atmosphere.SkyBox_SetScale(5, 5, 5);
            Name = "SkyBox";
            UniqueId = Name;
            Core.IsSkyBox = true;
            Renderable = false;
        }

        ~SkyBox()
        {
            core.Atmosphere.SkyBox_Enable(false);
            Core.IsSkyBox = false;
        }

        private void SetTextures()
        {
            core.Atmosphere.SkyBox_SetTexture(core.TextureFactory.LoadTexture(front),
                core.TextureFactory.LoadTexture(back),
                core.TextureFactory.LoadTexture(left),
                core.TextureFactory.LoadTexture(right),
                core.TextureFactory.LoadTexture(top),
                core.TextureFactory.LoadTexture(bottom));

            FrontFileName = GetPath(front);
            BackFileName = GetPath(back);
            LeftFileName = GetPath(left);
            RightFileName = GetPath(right);
            TopFileName = GetPath(top);
            BottomFileName = GetPath(bottom);
        }

        private string GetPath(string texture)
        {
            if (texture == null)
                return string.Empty;
            else
                return texture.Replace(string.Format("{0}\\", Application.StartupPath), string.Empty);
        }

        public override void Update()
        {
        }

        public override void Select()
        {
        }

        public override void Deselect()
        {
        }

        public override void GetBoundingBox(ref TV_3DVECTOR min, ref TV_3DVECTOR max)
        {
        }

        public override TVMesh GetMesh()
        {
            return null;
        }
    }
}
