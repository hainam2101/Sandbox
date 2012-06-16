using System.ComponentModel;
using MTV3D65;
using Sandbox.Properties;

namespace Engine 
{
    public class Water : ObjectBase
    {
        private ICore core;
        private readonly TVMesh mesh;
        [Browsable(false)]
        public TVRenderSurface ReflectRS { get; internal set; }
        [Browsable(false)]
        public TVRenderSurface RefractRS { get; internal set; }
        private TV_PLANE plane;
        [Browsable(false)]
        public override bool Visible { get; set; }
        [Browsable(false)]
        public override string FileName { get; set; }

        public Water(ICore core)
            : base(core)
        {
            this.core = core;

            ReflectRS = core.Scene.CreateRenderSurfaceEx(-1, -1, CONST_TV_RENDERSURFACEFORMAT.TV_TEXTUREFORMAT_DEFAULT, true, true, 1);
            ReflectRS.SetBackgroundColor(core.Globals.RGBA(0f, 0f, 0.1906f, 1f));

            RefractRS = core.Scene.CreateRenderSurfaceEx(-1, -1, CONST_TV_RENDERSURFACEFORMAT.TV_TEXTUREFORMAT_DEFAULT, true, true, 1);
            RefractRS.SetBackgroundColor(core.Globals.RGBA(0f, 0f, 0.1906f, 1f));

            mesh = core.Scene.CreateMeshBuilder();
            mesh.AddFloor(Helpers.GetDUDVTextureFromResource(core, Resources.water), -256, -256, 256, 256, -3, 2, 2);
            
            plane = new TV_PLANE(core.Globals.Vector3(0, 1, 0), 3f);
            core.GraphicEffect.SetWaterReflection(mesh, ReflectRS, RefractRS, 0, plane);

            Name = core.GetName<Water>();
            UniqueId = mesh.GetMeshName();
        }

        public override void Update()
        {
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
