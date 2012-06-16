using MTV3D65;

namespace Engine
{
    public delegate void Update();

    public interface IObjectBase
    {
        string UniqueId { get; }
        string FileName { get; }
        VECTOR3D Position { get; }
        VECTOR3D Rotation { get; }
        VECTOR3D Scale { get; }
        bool Selected { get; }

        void Update();
        void Select();
        void Deselect();
        void GetBoundingBox(ref TV_3DVECTOR min, ref TV_3DVECTOR max);
        TVMesh GetMesh();
    }
}