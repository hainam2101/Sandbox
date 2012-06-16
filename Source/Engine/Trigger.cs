using System.ComponentModel;
using System.Drawing.Design;
using MTV3D65;

namespace Engine
{
    public class Trigger : ObjectBase
    {
        private ICore core;
        private readonly TVMesh mesh;

        [Browsable(false)]
        public override bool Visible { get; set; }
        [Browsable(false)]
        public override string FileName { get; set; }

        [EditorAttribute(typeof(MyColorEditor), typeof(UITypeEditor))]
        [Category("Base")]
        public MyColor Color { get; set; }

        public Trigger(ICore core)
            : base(core)
        {
            this.core = core;
            Name = core.GetName<Trigger>();
            Color = new MyColor(0, 255, 0);

            mesh = core.Scene.CreateMeshBuilder();
            mesh.CreateBox(1f, 1f, 1f, false);
            mesh.SetColor(core.Globals.RGBA(Color.R / 255f, Color.G / 255f, Color.B / 255f, 1));
            mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
            mesh.SetAlphaTest(true);
            mesh.SetBlendingMode(CONST_TV_BLENDINGMODE.TV_BLEND_ADD);

            UniqueId = mesh.GetMeshName();
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
            return mesh;
        }

        public override void Update()
        {
            mesh.SetPosition(Position.X, Position.Y, Position.Z);
            mesh.SetRotation(Rotation.X, Rotation.Y, Rotation.Z);
            mesh.SetScale(Scale.X, Scale.Y, Scale.Z);
            mesh.SetColor(core.Globals.RGBA(Color.R / 255f, Color.G / 255f, Color.B / 255f, 1));
        }
    }
}
