using System.ComponentModel;
using System.Windows.Forms;
using MTV3D65;
using Sandbox;
using Sandbox.Properties;

namespace Engine
{
    public class SkySphere : ObjectBase
    {
        private ICore core;
        private string texture;

        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sky sphere texture.")]
        public string Texture { get { return Helpers.IsTextureFromMemory(texture) ? string.Empty : texture; } set { texture = value; SetTexture(); } }

        [Browsable(false)]
        public override VECTOR3D Position { get; set; }
        [Browsable(false)]
        public override bool Visible { get; set; }
        [Browsable(false)]
        public override string FileName { get; set; }

        [Category("Transform"), DisplayName("Poly count")]
        public int PolyCount { get; set; }

        public SkySphere(ICore core)
            : base(core)
        {
            this.core = core;
            Texture = Helpers.GetTextureSourceFromResource(core, Resources.defaultTexture);
            core.Atmosphere.SkySphere_Enable(true);
            core.Atmosphere.SkyBox_Enable(false);
            core.Atmosphere.SkySphere_SetScale(1, 1, 1);
            Name = "SkySphere";
            UniqueId = Name;
            Core.IsSkySphere = true;
            Renderable = false;

            float scaleX = 0, scaleY = 0, scaleZ = 0;
            core.Atmosphere.SkySphere_GetScale(ref scaleX, ref scaleY, ref scaleZ);
            Scale = new VECTOR3D(scaleX, scaleY, scaleZ);

            TV_3DVECTOR rotation = core.Atmosphere.SkySphere_GetRotation();
            Rotation = new VECTOR3D(rotation.x, rotation.y, rotation.z);

            PolyCount = core.Atmosphere.SkySphere_GetPolyCount();
        }

        ~SkySphere()
        {
            core.Atmosphere.SkySphere_Enable(false);
            Core.IsSkySphere = false;
        }

        private void SetTexture()
        {
            core.Atmosphere.SkySphere_SetTexture(core.TextureFactory.LoadTexture(texture));
            FileName = Texture.Replace(string.Format("{0}\\", Application.StartupPath), string.Empty);
        }

        public override void Update()
        {
            core.Atmosphere.SkySphere_SetScale(Scale.X, Scale.Y, Scale.Z);
            core.Atmosphere.SkySphere_SetRotation(Rotation.X, Rotation.Y, Rotation.Z);
            core.Atmosphere.SkySphere_SetPolyCount(PolyCount);
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
