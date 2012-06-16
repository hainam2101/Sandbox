using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using MTV3D65;
using Sandbox.Properties;

namespace Engine
{
    public class Particle : ObjectBase
    {
        private ICore core;
        private readonly TVMesh mesh;
        private TVParticleSystem particle;

        //[Browsable(false)]
        //public override VECTOR3D Rotation { get; set; }

        public Particle(ICore core, string fileName)
            : base(core)
        {
            this.core = core;
            FileName = fileName;
            Visible = true;
            Name = core.GetName<Particle>();

            TV_3DVECTOR position = Core.Camera.GetFrontPosition(10.0f);
            Position = new VECTOR3D(position.x, position.y, position.z);
            mesh = core.Scene.CreateBillboard(Helpers.GetTextureFromResource(core, Resources.particleBig), position.x, position.y,
                                              position.z, 1.0f, 1.0f);
            mesh.SetAlphaTest(true);

            particle = core.Scene.CreateParticleSystem();
            particle.Load(fileName);
            particle.SetGlobalPosition(Position.X, Position.Y, Position.Z);
            particle.SetGlobalRotation(Rotation.X, Rotation.Y, Rotation.Z);
            particle.SetGlobalScale(Scale.X, Scale.Y, Scale.Z);
            
            UniqueId = mesh.GetMeshName();
        }

        public void Destroy()
        {
            particle.Destroy();
            particle = null;
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
            particle.SetGlobalPosition(Position.X, Position.Y, Position.Z);
            particle.SetGlobalRotation(Rotation.X, Rotation.Y, Rotation.Z);
            particle.SetGlobalScale(Scale.X, Scale.Y, Scale.Z);
            particle.Enable(Visible);
        }
    }
}
