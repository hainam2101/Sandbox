using System.ComponentModel;
using IrrKlang;
using MTV3D65;
using Sandbox.Properties;

namespace Engine
{
    public class Sound : ObjectBase
    {
        private readonly TVMesh mesh;
        private Engine.SoundFactory.MySound mySound;

        [Browsable(false)]
        public override VECTOR3D Rotation { get; set; }
        [Browsable(false)]
        public override VECTOR3D Scale { get; set; }
        [Browsable(false)]
        public override bool Visible { get; set; }

        [Category("Base")]
        public bool Stopped { get; set; }
        [Category("Base")]
        public bool Loop { get; set; }
        [Category("Base"), Description("Sound volume in %. Min value 0, max 100.")]
        public int Volume 
        { 
            get 
            { 
                return volume; 
            } 
            set 
            {
                if (value > 100)
                    volume = 100;
                else if (value < 0)
                    volume = 0;
                else
                    volume = value;
            } 
        }
        private int volume;

        [Category("Base"), Description("Determines whether sound is 2d or 3d.")]
        public bool Is3D
        {
            get;
            set;
        }
        
        public Sound(ICore core, string fileName)
            : base(core)
        {
            FileName = fileName;
            Name = core.GetName<Sound>();
            
            TV_3DVECTOR position = Core.Camera.GetFrontPosition(10.0f);
            Position = new VECTOR3D(position.x, position.y, position.z);
            Is3D = false;
            mySound = core.SoundFactory.Load(fileName, Is3D);
            core.SoundFactory.StopAllSounds();
            mesh = core.Scene.CreateBillboard(Helpers.GetTextureFromResource(core, Resources.sound), position.x, position.y,
                                              position.z, 1.0f, 1.0f);
            mesh.SetAlphaTest(true); 
            Stopped = true;
            Loop = false;
            volume = 100;
            UniqueId = mesh.GetMeshName();
        }

        public override void Update()
        {
            mesh.SetPosition(Position.X, Position.Y, Position.Z);
            mySound.Position = new Vector3D(Position.X, Position.Y, Position.Z);
            mySound.Stopped = Stopped;
            mySound.Loop = Loop;
            mySound.Is3D = Is3D;
            mySound.Volume = (float)volume / 100;
            Core.SoundFactory.Update(mySound);
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
        }

        public override TVMesh GetMesh()
        {
            if (mesh != null)
                return mesh;
            else
                return default(TVMesh);
        }

        public Engine.SoundFactory.MySound GetSound()
        {
            return mySound;
        }
    }
}