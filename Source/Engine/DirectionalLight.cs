using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using MTV3D65;
using Sandbox.Properties;

namespace Engine
{
    public class DirectionalLight : ObjectBase
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
        public override VECTOR3D Position { get; set; }
        [Category("Base"), ReadOnly(true)]
        public VECTOR3D Direction { get; set; }

        public DirectionalLight(ICore core, TV_3DVECTOR position)
            : base(core)
        {
            LightId = -1;
            Position = new VECTOR3D(position.x, position.y, position.z);
            Direction = GetDirection();
            this.Color = new MyColor();
            mesh = core.Scene.CreateMeshBuilder();
            mesh = Core.Scene.CreateBillboard(Helpers.GetTextureFromResource(core, Resources.directionalLight), position.x, position.y,
                                             position.z, 1.0f, 1.0f);
            mesh.SetAlphaTest(true);
            UniqueId = mesh.GetMeshName();

            LightId = core.LightEngine.CreateDirectionalLight(new TV_3DVECTOR(Direction.X, Direction.Y, Direction.Z), Color.R / 255f, this.Color.G / 255f, this.Color.B / 255f, Helpers.SUN);
            //core.LightEngine.SetLightAttenuation(LightId, 1f, 0f, 0f);
            //core.LightEngine.SetSpecularLighting(true);
            //core.LightEngine.EnableLight(LightId, true);
            Core.LightEngine.SetLightProperties(LightId, true, true, true);
            Name = "DirectionalLight";
        }

        private VECTOR3D GetDirection()
        {
            TV_3DVECTOR camPos = Core.Camera.GetPosition();
            TV_3DVECTOR camLookAt = Core.Camera.GetLookAt();

            Core.Camera.SetPosition(Position.X, Position.Y, Position.Z);
            Core.Camera.SetLookAt(0f, 0f, 0f);
            TV_3DVECTOR camDirection = Core.Camera.GetDirection();
            Core.MathLibrary.TVVec3Normalize(ref camDirection, camDirection);
            Core.Camera.SetPosition(camPos.x, camPos.y, camPos.z);
            Core.Camera.SetLookAt(camLookAt.x, camLookAt.y, camLookAt.z);

            return new VECTOR3D(camDirection.x, camDirection.y, camDirection.z);
        }

        public override void Update()
        {
            mesh.SetPosition(Position.X, Position.Y, Position.Z);
            Direction = GetDirection();
            Core.LightEngine.SetLightDirection(LightId, Direction.X, Direction.Y, Direction.Z);
            Core.LightEngine.SetLightColor(LightId, Color.R / 255f, this.Color.G / 255f, this.Color.B / 255f);
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
    }
}
