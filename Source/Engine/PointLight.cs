using System.ComponentModel;
using System.Drawing.Design;
using MTV3D65;
using Sandbox.Properties;

namespace Engine
{
    public class PointLight : ObjectBase
    {
        private readonly TVMesh mesh;
        public readonly int LightId;

        [Browsable(false)]
        public override string FileName { get; set; }
        [Browsable(false)]
        public override VECTOR3D Rotation { get; set; }
        [Browsable(false)]
        public override VECTOR3D Scale { get; set; }

        [EditorAttribute(typeof(MyColorEditor), typeof(UITypeEditor))]
        [Category("Base")]
        public MyColor Color { get; set; }
        [Category("Base")]
        public float Radius { get; set; }

        public PointLight(ICore core, TV_3DVECTOR position)
            : base(core)
        {
            Position = new VECTOR3D(position.x, position.y, position.z);
            this.Color = new MyColor();
            mesh = core.Scene.CreateMeshBuilder();
            mesh = Core.Scene.CreateBillboard(Helpers.GetTextureFromResource(core, Resources.pointLight), position.x, position.y,
                                             position.z, 1.0f, 1.0f);
            mesh.SetAlphaTest(true);

            UniqueId = mesh.GetMeshName();
            Radius = 30f;
            LightId = core.LightEngine.CreatePointLight(position, 1f, 1f, 1f, Radius);
            Core.LightEngine.SetLightProperties(LightId, true, true, true);
            Name = core.GetName<PointLight>();
        }

        public override void Update()
        {
            mesh.SetPosition(Position.X, Position.Y, Position.Z);
            Core.LightEngine.SetLightPosition(LightId, Position.X, Position.Y, Position.Z);
            Core.LightEngine.SetLightRange(LightId, Radius);
            Core.LightEngine.SetLightColor(LightId, this.Color.R / 255f, this.Color.G / 255f, this.Color.B / 255f);
            Core.LightEngine.EnableLight(LightId, Visible);
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

        public override void GetBoundingBox(ref TV_3DVECTOR min, ref TV_3DVECTOR max)
        {
            mesh.GetBoundingBox(ref min, ref max);
        }

        public override TVMesh GetMesh()
        {
            if (mesh != null)
                return mesh;
            else
                return default(TVMesh);
        }

        public void HideMesh()
        {
            
        }
    }
}
