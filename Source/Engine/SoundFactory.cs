using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IrrKlang;
using MTV3D65;

namespace Engine
{
    public class SoundFactory
    {
        public ISoundEngine SoundEngine { get; set; }
        private List<MySound> allSounds;

        public class MySound
        {
            public readonly string Id;
            public ISound Sound;
            public ISoundSource Source;
            public bool Loop;
            public bool Stopped;
            public float Volume;
            public bool Is3D;
            public Vector3D Position;

            public bool IsPlaying;

            public MySound(ISound sound, ISoundSource source)
            {
                Id = Guid.NewGuid().ToString();
                Sound = sound;
                Source = source;
                Loop = true;
                Stopped = false;
                Volume = 1.0f;
                Position = new Vector3D(0f, 0f, 0f);
                IsPlaying = false;
            }

            public void Update(MySound mySound)
            {

                Position = mySound.Position;
                Loop = mySound.Loop;
                Stopped = mySound.Stopped;
                Volume = mySound.Volume;
                if (Sound != null)
                    Sound.Volume = Volume;
            }

            public void Play(ISoundEngine soundEngine)
            {
                if (!Stopped && !IsPlaying /*!soundEngine.IsCurrentlyPlaying(Source.Name)*/)
                {
                    IsPlaying = true;
                    switch (Is3D)
                    {
                        case true:
                            soundEngine.Play3D(Source, Position.X, Position.Y, Position.Z, Loop, false, false);
                            break;
                        default:
                            soundEngine.Play2D(Source, Loop, false, false);
                            break;
                    }
                }
            }
        }

        public SoundFactory()
        {
            SoundEngine = new ISoundEngine();
            allSounds = new List<MySound>();
        }

        ~SoundFactory()
        {
            allSounds = null;
            SoundEngine.StopAllSounds();
            SoundEngine = null;
        }

        public void StopAllSounds()
        {
            SoundEngine.StopAllSounds();
            allSounds.FindAll(s => s.IsPlaying = false);
        }

        public void PlaySound(MySound sound)
        {
            allSounds.FindLast(ms => ms.Id.Equals(sound.Id)).Play(SoundEngine);
        }

        public void StartAllSounds()
        {
            foreach (MySound mySound in allSounds)
            {
                mySound.Play(SoundEngine);
            }
        }

        public void RemoveAllSounds()
        {
            StopAllSounds();
            SoundEngine.RemoveAllSoundSources();
            allSounds = new List<MySound>();
        }

        public void RemoveSound(MySound mySound)
        {
            StopAllSounds();
            allSounds.RemoveAll(ms => ms.Equals(mySound));
        }

        public MySound Load(string fileName, bool is3d)
        {
            var position = new Vector3D(0f, 0f, 0f);
            var source = SoundEngine.GetSoundSource(fileName, true);
            ISound sound = null;

            switch (is3d)
            {
                case true:
                    sound = SoundEngine.Play3D(source, position.X, position.Y, position.Z, true, true, false);
                    break;
                default:
                    sound = SoundEngine.Play2D(source, true, true, false);
                    break;
            }

            if (sound == null)
            {
                MessageBox.Show("Unsupported audio file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MySound mySound = new MySound(sound, source);
            allSounds.Add(mySound);
            return mySound;
        }

        public void Update(MySound mySound)
        {
            allSounds.FindLast(ms => ms.Id.Equals(mySound.Id)).Update(mySound);
        }

        public void Update(TVCamera camera)
        {
            TV_3DVECTOR position = camera.GetPosition();
            TV_3DVECTOR lookDir = camera.GetLookAt() - camera.GetPosition();
            SoundEngine.SetListenerPosition(new Vector3D(position.x, position.y, position.z),
                                            new Vector3D(lookDir.x, lookDir.y, lookDir.z).Normalize());
        }

        public bool IsPlaying(MySound sound)
        {
            return !sound.Sound.Finished;
        }

        public void StopSound(MySound sound)
        {
            sound.Sound.Stop();
        }
    }
}