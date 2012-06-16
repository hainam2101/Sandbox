using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MTV3D65;
using Sandbox;
using Sandbox.Properties;

namespace Engine
{
    public class Mesh : ObjectBase
    {
        private ICore core;
        private readonly TVMesh mesh;
        private int materialIdx;
        private ProgramSettings settings;
        public int PhysicsId;
        private int lightmapIdx;

        private enum LightMode
        {
            None,
            Managed,
            Normal,
            Offset
        }

        public enum Materials
        {
            Custom,
            Wood,
            Metal,
            Concrete
        }

        [Category("Base")]
        [Description("Defines if this is an animated object.")]
        public bool IsAnimated { get; set; }

        private UV textureScale;
        [Category("Texture"), DisplayName("Scale"), Description("Scale texture.")]
        public UV TextureScale { get { return textureScale; } set { textureScale = value; SetTextureScale(); } }

        private Materials material;
        [Category("Physics"), Description("Material.")]
        public Materials Material { get { return material; } set { material = value; UpdatePhysics(); } }

        private float mass;
        [Category("Physics"), Description("Object mass.")]
        public float Mass { get { return mass; } set { mass = value; } }

        [Category("Physics"), Description("Bounding type.")]
        public Helpers.BodyBounding Bounding { get; set; }

        private float staticFriction;
        [Category("Physics"), Description("Static friction value.")]
        public float StaticFriction { get { return staticFriction; } set { staticFriction = value; SetMaterialToCustom(); } }

        private float kineticFriction;
        [Category("Physics"), Description("Kinetic friction value.")]
        public float KineticFriction { get { return kineticFriction; } set { kineticFriction = value; SetMaterialToCustom(); } }

        private float softness;
        [Category("Physics"), Description("Softness value.")]
        public float Softness { get { return softness; } set { softness = value; SetMaterialToCustom(); } }

        private float bounciness;
        [Category("Physics"), Description("Bounciness value.")]
        public float Bounciness { get { return bounciness; } set { bounciness = value; SetMaterialToCustom(); } }

        private bool enableLightning;
        [Category("Base"), Description("Enable lightning.")]
        public bool EnableLightning { get { return enableLightning; } set { enableLightning = value; ChangeLightning(); } }

        private string customTexture;
        [Category("Texture"), Editor(typeof(TexturesFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Custom diffuse texture.")]
        public string CustomTexture { get { return customTexture; } set { customTexture = value; SetCustomTexture(); } }

        public Mesh(ICore core, ProgramSettings settings, string fileName)
            : base(core)
        {
            this.core = core;
            this.settings = settings;

            mass = 0f;
            staticFriction = 0.9f;
            kineticFriction = 0.5f;
            softness = 0.1f;
            bounciness = 0.1f;
            materialIdx = -1;
            SetMaterialToCustom();

            FileName = fileName;
            customTexture = string.Empty;
            //Name = fileName.Split(new[] { '\\' }).Last();
            PhysicsId = -1;

            enableLightning = true;

            Name = core.GetName<Mesh>();
            string ending = fileName.Split(new[] { '\\' }).Last().ToUpper();
            mesh = core.Scene.CreateMeshBuilder();

            if (ending.EndsWith(Helpers.GetFileExtension(Helpers.FileFormat.TVM)))
            {
                mesh.LoadTVM(fileName, true, false);
            }
            else if (ending.EndsWith(Helpers.GetFileExtension(Helpers.FileFormat.X)))
            {
                mesh.LoadXFile(fileName, true, false);
            }
            else if (ending.EndsWith(Helpers.GetFileExtension(Helpers.FileFormat.TVA)))
            {
                TVActor actor = core.Scene.CreateActor();
                actor.Load(fileName, true, false);
                mesh = actor.GetDeformedMesh();
                core.Scene.DestroyAllActors();
                IsAnimated = true;
            }
            else
                return;

            mesh.EnableFrustumCulling(true, true);
            mesh.ComputeNormals();
            mesh.ComputeBoundings();
            mesh.ComputeOctree();
            mesh.SetCullMode(CONST_TV_CULLING.TV_BACK_CULL);
            mesh.SetBlendingMode(CONST_TV_BLENDINGMODE.TV_BLEND_ALPHA);
            mesh.SetAlphaTest(true);
            mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
            mesh.SetShadowCast(true, true);
            lightmapIdx = mesh.GetTextureEx((int)CONST_TV_LAYER.TV_LAYER_LIGHTMAP);

            textureScale = new UV(1.0f, 1.0f);

            LoadTextures();

            UniqueId = mesh.GetMeshName();
        }

        private void SetMaterialToCustom()
        {
            material = Materials.Custom;
        }

        private void SetTextureScale()
        {
            mesh.SetTextureModEnable(true);
            mesh.SetTextureModTranslationScale(0, 0, TextureScale.U, TextureScale.V);
        }

        private void LoadTextures()
        {
            if (!settings.UseAdvanceTexturing)
            {
                // Set default texture for mesh with no textures.
                for (int i = 0; i < mesh.GetGroupCount(); i++)
                {
                    TV_TEXTURE textureInfo = Core.TextureFactory.GetTextureInfo(mesh.GetTexture(i));
                    if (textureInfo.Name.Equals(Helpers.TEXTURE_BLANK))
                    {
                        SetDefaultTexture(mesh, i);
                    }
                }
            }
            else if (!enableLightning)
            {
                mesh.SetMaterial(0);
                mesh.SetTextureEx((int)CONST_TV_LAYER.TV_LAYER_LIGHTMAP, lightmapIdx);
                mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
                mesh.SetShadowCast(false, false);
            }
            else
            {
                mesh.SetShadowCast(true, true);
                var lightMode = LightMode.None;

                int defaultNormalTexId = -1;
                int defaultSpecularTexId = -1;

                for (var i = 0; i < mesh.GetGroupCount(); i++)
                {
                    var textureInfo = Core.TextureFactory.GetTextureInfo(mesh.GetTexture(i));

                    var normalTexture = string.Empty;
                    var normalTextureName = string.Empty;
                    var specularTexture = string.Empty;
                    var specularTextureName = string.Empty;
                    var heightTexture = string.Empty;
                    var heightTextureName = string.Empty;

                    if (textureInfo.Name.Equals(Helpers.TEXTURE_BLANK))
                    {
                        Helpers.SetTextureFromResource(core, mesh, Resources.defaultTexture, i);
                    }
                    else if (Helpers.IsTextureFromMemory(textureInfo.Filename))
                    {
                        mesh.SetTexture(Core.TextureFactory.LoadTexture(textureInfo.Name), i);
                    }
                    else
                    {
                        var texture = textureInfo.Filename;

                        var fileInfo = new FileInfo(textureInfo.Filename);
                        normalTextureName = fileInfo.Name.Replace(fileInfo.Extension,
                                                                            settings.TextureNormalSuffix +
                                                                            fileInfo.Extension);
                        normalTexture = Directory.GetParent(textureInfo.Filename) + @"\" + normalTextureName;
                        heightTextureName = fileInfo.Name.Replace(fileInfo.Extension,
                                                                            settings.TextureHeightSuffix +
                                                                            fileInfo.Extension);
                        heightTexture = Directory.GetParent(textureInfo.Filename) + @"\" + heightTextureName;
                        specularTextureName = fileInfo.Name.Replace(fileInfo.Extension,
                                                                              settings.TextureSpecularSuffix +
                                                                              fileInfo.Extension);
                        specularTexture = Directory.GetParent(textureInfo.Filename) + @"\" + specularTextureName;
                        //mesh.SetTextureEx((int)CONST_TV_LAYER.TV_LAYER_BASETEXTURE, core.Globals.GetTex(textureInfo.Name), i);
                    }

                    var normalId = -1;
                    if (File.Exists(normalTexture))
                    {
                        normalId = core.TextureFactory.LoadTexture(normalTexture, normalTextureName);
                    }
                    else
                    {
                        if (defaultNormalTexId == -1)
                        {
                            normalId = Helpers.LoadTextureFromResourceToMemory(core, Resources.defaultNormal);
                            defaultNormalTexId = normalId;
                        }
                        else
                        {
                            normalId = defaultNormalTexId;
                        }
                    }

                    mesh.SetTextureEx((int)CONST_TV_LAYER.TV_LAYER_NORMALMAP, normalId, i);

                    if (lightMode != LightMode.Offset)
                    {
                        lightMode = LightMode.Normal;
                    }

                    #region Set specular texture
                    // http://www.truevision3d.com/forums/empty-t19109.0.html;msg131232#msg131232
                    var specularId = -1;

                    if (File.Exists(specularTexture))
                    {
                        specularId = Core.TextureFactory.AddAlphaChannel(normalId,
                                                                      Core.TextureFactory.LoadTexture(specularTexture),
                                                                      specularTexture);
                    }
                    else
                    {
                        if (defaultSpecularTexId == -1)
                        {
                            defaultSpecularTexId = Helpers.LoadTextureFromResourceToMemory(core, Resources.defaultSpecular);
                        }

                        specularId = Core.TextureFactory.AddAlphaChannel(normalId, defaultSpecularTexId, "defaultSpecular");
                    }

                    mesh.SetTextureEx((int)CONST_TV_LAYER.TV_LAYER_SPECULARMAP, specularId, i);
                    #endregion

                    if (File.Exists(heightTexture))
                    {
                        mesh.SetTextureEx((int)CONST_TV_LAYER.TV_LAYER_HEIGHTMAP,
                                          Core.TextureFactory.LoadTexture(heightTexture), i);
                        lightMode = LightMode.Offset;
                    }

                    if (core.MaterialIdx == -1)
                    {
                        TV_COLOR ambient = new TV_COLOR(0.25f, 0.25f, 0.25f, 1f);
                        TV_COLOR diffuse = new TV_COLOR(1f, 1f, 1f, 1f);
                        TV_COLOR specular = new TV_COLOR(0.35f, 0.35f, 0.35f, 1f);
                        TV_COLOR emissive = new TV_COLOR(1f, 1f, 1f, 1f);
                        float power = 100;

                        core.MaterialIdx = Core.MaterialFactory.CreateMaterial(Guid.NewGuid().ToString());
                        core.MaterialFactory.SetAmbient(core.MaterialIdx, ambient.r, ambient.g, ambient.b, ambient.a);
                        core.MaterialFactory.SetDiffuse(core.MaterialIdx, diffuse.r, diffuse.g, diffuse.b, diffuse.a);
                        core.MaterialFactory.SetSpecular(core.MaterialIdx, specular.r, specular.g, specular.b, specular.a);
                        core.MaterialFactory.SetEmissive(core.MaterialIdx, emissive.r, emissive.g, emissive.b, emissive.a);
                        core.MaterialFactory.SetPower(core.MaterialIdx, power);
                    }

                    mesh.SetMaterial(core.MaterialIdx, i);
                }

                switch (lightMode)
                {
                    case LightMode.Normal:
                        mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_BUMPMAPPING_TANGENTSPACE);
                        break;
                    case LightMode.Offset:
                        mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_OFFSETBUMPMAPPING_TANGENTSPACE);
                        break;
                    case LightMode.Managed:
                        mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
                        break;
                    default:
                        mesh.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_NONE);
                        break;
                }
            }
        }

        private void SetDefaultTexture(TVMesh mesh, int groupID)
        {
            Bitmap defaultTexture = Resources.defaultTexture;
            var ms = new MemoryStream();
            defaultTexture.Save(ms, ImageFormat.Png);
            var data = new byte[ms.Length];
            ms.Seek(0, 0);
            data = ms.ToArray();
            ms.Dispose();
            defaultTexture.Dispose();
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            int addr = handle.AddrOfPinnedObject().ToInt32();
            handle.Free();
            string ds = Core.Globals.GetDataSourceFromMemory(addr, data.Length - 1);
            int texture = Core.TextureFactory.LoadTexture(ds);
            mesh.SetTexture(texture, groupID);
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
            return mesh;
        }

        public void UpdatePhysics()
        {
            if (mesh == null)
                return;

            if (PhysicsId != -1)
            {
                core.Physics.DestroyBody(PhysicsId, false);
            }

            if (mass == 0f)
            {
                if (!Bounding.Equals(Helpers.BodyBounding.None))
                {
                    PhysicsId = core.Physics.CreateStaticMeshBody(mesh);
                }
                else
                {
                    PhysicsId = -1;
                    return;
                }
            }
            else
            {
                CONST_TV_PHYSICSBODY_BOUNDING bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_CONVEXHULL;
                switch (Bounding)
                {
                    case Helpers.BodyBounding.Box:
                        bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_BOX;
                        break;
                    case Helpers.BodyBounding.Convexhull:
                        bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_CONVEXHULL;
                        break;
                    case Helpers.BodyBounding.Cylinder:
                        bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_CYLINDER;
                        break;
                    case Helpers.BodyBounding.None:
                        bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_NONE;
                        break;
                    case Helpers.BodyBounding.Sphere:
                        bounding = CONST_TV_PHYSICSBODY_BOUNDING.TV_BODY_SPHERE;
                        break;
                }

                PhysicsId = core.Physics.CreateMeshBody(mass, mesh, bounding, true);
            }

            // Set material from selected material template
            switch (material)
            {
                case Materials.Custom:
                    //Do nothing
                    break;
                case Materials.Wood:
                    staticFriction = 0.5f;
                    kineticFriction = 0.2f;
                    softness = 0.1f;
                    bounciness = 0.1f;
                    break;
                case Materials.Metal:
                    staticFriction = 1f;
                    kineticFriction = 0.7f;
                    softness = 0.1f;
                    bounciness = 0.1f;
                    break;
                case Materials.Concrete:
                    staticFriction = 0.7f;
                    kineticFriction = 0.4f;
                    softness = 0.1f;
                    bounciness = 0.1f;
                    break;
            }

            // Create material
            if (materialIdx == -1)
            {
                //materialIdx = Core.Physics.CreateMaterialGroup(Guid.NewGuid().ToString());
            }

            if (PhysicsId != -1)
            {
                //Core.Physics.SetMaterialInteractionFriction(materialIdx, -1, staticFriction, kineticFriction);
                //Core.Physics.SetMaterialInteractionSoftness(materialIdx, -1, softness);
                //Core.Physics.SetMaterialInteractionBounciness(materialIdx, -1, bounciness);
                //Core.Physics.SetBodyMaterialGroup(PhysicsId, materialIdx);

                Core.Physics.SetAutoFreeze(PhysicsId, false);
                Core.Physics.SetBodyPosition(PhysicsId, Position.X, Position.Y, Position.Z);
                Core.Physics.SetBodyRotation(PhysicsId, Rotation.X, Rotation.Y, Rotation.Z);
                Core.Physics.SetDamping(PhysicsId, 0f, new TV_3DVECTOR(0f, 0f, 0f));
            }
        }

        private void ChangeLightning()
        {
            LoadTextures();
        }

        private void SetCustomTexture()
        {
            if (!customTexture.Equals(string.Empty))
            {
                mesh.SetTexture(Core.TextureFactory.LoadTexture(customTexture));
                LoadTextures();
            }
        }
    }
}