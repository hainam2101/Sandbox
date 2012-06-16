using System.Collections.Generic;
using MTV3D65;
using Sandbox;

namespace Engine
{
    public interface ICore
    {
        TVEngine Engine { get; }
        TVScene Scene { get; }
        TVCamera Camera { get; }
        TVGlobals Globals { get; }
        TVInputEngine Input { get; }
        TVViewport Viewport { get; }
        TVMathLibrary MathLibrary { get; }
        TVTextureFactory TextureFactory { get; }
        TVLightEngine LightEngine { get; }
        TVMaterialFactory MaterialFactory { get; }
        TVGraphicEffect GraphicEffect { get; }
        //TVRenderSurface RenderSurface { get; }
        TVScreen2DImmediate Screen2DImmediate { get; }
        TVCollisionResult CollisionResult { get; }
        TVShader Shader { get; }
        TVAtmosphere Atmosphere { get; }
        TVPhysics Physics { get; }
        TVInternalObjects InternalObjects { get; }
        SoundFactory SoundFactory { get; }
        List<ObjectBase> AllObjects { get; }
        string GetName<T>() where T : ObjectBase;
        ProgramSettings Settings { get; }

        int MaterialIdx { get; set; }
        bool IsSkySphere { get; set; }
        bool IsSkyBox { get; set; }
        bool PreviewingScene { get; set; }
        bool LoadingScene { get; set; }
    }
}