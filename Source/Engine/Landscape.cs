using System.ComponentModel;
using System.Windows.Forms;
using Engine;
using MTV3D65;
using Sandbox.Properties;

namespace Sandbox
{
    public class Landscape : ObjectBase
    {
        private ICore core;
        private readonly TVMesh mesh;
        private TVLandscape land;
        private int landTextureIdx, heightTextureIdx;
        private string landTexture, heightTexture;

        [Browsable(false)]
        public override bool Visible { get; set; }
        [Browsable(false)]
        public override string FileName { get; set; }
        [Browsable(false)]
        public string LandTextureFileName, HeightTextureFileName;

        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Landscape texture.")]
        public string LandTexture { get { return landTexture; } set { landTexture = value; SetTexture(); } }
        [Category("Base"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Height texture.")]
        public string HeightTexture { get { return heightTexture; } set { heightTexture = value; SetTexture(); } }

        public Landscape(ICore core)
            : base(core)
        {
            this.core = core;
            landTextureIdx = -1;
            heightTextureIdx = -1;

            landTextureIdx = Helpers.GetTextureFromResource(core, Resources.defaultTexture);

            land = core.Scene.CreateLandscape();
            land.GenerateTerrain(Helpers.GetTextureSourceFromResource(core, Resources.defaultHeight), CONST_TV_LANDSCAPE_PRECISION.TV_PRECISION_HIGH, 8, 8, 0, 0, 0);
            land.SetCullMode(CONST_TV_CULLING.TV_BACK_CULL);
            land.ExpandTexture(landTextureIdx, 0, 0, 8, 8);

            mesh = core.Scene.CreateMeshBuilder();

            Name = "Landscape";
            UniqueId = land.GetName();
        }

        private void SetTexture()
        {
            if (!string.IsNullOrEmpty(landTexture))
            {
                landTextureIdx = core.TextureFactory.LoadTexture(landTexture);
                LandTextureFileName = GetPath(landTexture);
            }

            if (!string.IsNullOrEmpty(heightTexture))
            {
                heightTextureIdx = core.TextureFactory.LoadTexture(heightTexture);
                HeightTextureFileName = GetPath(heightTexture);
            }

            core.Scene.DestroyAllLandscapes();
            land = core.Scene.CreateLandscape();
            land.GenerateTerrain(heightTexture, CONST_TV_LANDSCAPE_PRECISION.TV_PRECISION_HIGH, 8, 8, 0, 0, 0);
            land.SetCullMode(CONST_TV_CULLING.TV_BACK_CULL);
            land.ExpandTexture(landTextureIdx, 0, 0, 8, 8);
            land.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
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
            land.SetPosition(Position.X, Position.Y, Position.Z);
            land.SetRotation(Rotation.X, Rotation.Y, Rotation.Z);
            land.SetScale(Scale.X, Scale.Y, Scale.Z);

            mesh.SetPosition(Position.X, Position.Y, Position.Z);
            mesh.SetRotation(Rotation.X, Rotation.Y, Rotation.Z);
            mesh.SetScale(Scale.X, Scale.Y, Scale.Z);
        }

        public override void Select()
        {
            mesh.ShowBoundingBox(true);
            Selected = true;
        }

        public override void Deselect()
        {
            mesh.ShowBoundingBox(false);
            Selected = false;
        }

        public override void GetBoundingBox(ref MTV3D65.TV_3DVECTOR min, ref MTV3D65.TV_3DVECTOR max)
        {
            //land.GetBoundingBox(ref min, ref max);
            mesh.GetBoundingBox(ref min, ref max);
        }

        public override TVMesh GetMesh()
        {
            if (mesh != null)
                return mesh;
            else
                return default(TVMesh);
        }
    }
}
