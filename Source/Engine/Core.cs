using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using MTV3D65;
using Sandbox;
using System.Xml.Linq;
using System.Linq;
using System.Collections;

namespace Engine
{
    public class Core : ICore
    {
        private readonly int fontID;
        private List<string> errorMessages;
        private ProgramSettings settings;

        public int MaterialIdx { get; set; }
        public bool IsSkySphere { get; set; }
        public bool IsSkyBox { get; set; }
        public bool PreviewingScene { get; set; }

        private struct ScriptInfo
        {
            public bool Enabled;
            public string Script;
        }

        public Core(IntPtr handle, ProgramSettings settings)
        {
            this.settings = settings;
            AllObjects = new List<ObjectBase>();
            MaterialIdx = -1;
            IsSkySphere = false;
            IsSkyBox = false;
            PreviewingScene = false;

            CONST_TV_MULTISAMPLE_TYPE antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_NONE;

            switch (settings.Antialiasing)
            {
                case ProgramSettings._Antialiasing.None:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_NONE;
                    break;
                case ProgramSettings._Antialiasing.X2:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_2_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X4:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_4_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X6:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_6_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X8:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_8_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X10:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_10_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X12:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_12_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X14:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_14_SAMPLES;
                    break;
                case ProgramSettings._Antialiasing.X16:
                    antialias = CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_16_SAMPLES;
                    break;
            }

            Engine = new TVEngine();
#if DEBUG
            Engine.SetDebugFile("Sandbox.log");
#else
            Engine.SetDebugMode(false, false, false, false);
#endif
            Engine.SetSearchDirectory(Path.Combine(Application.StartupPath, Helpers.PATH_DATA));
            Engine.Init3DWindowed(handle, true);
            Engine.GetViewport().SetAutoResize(true);
            Engine.DisplayFPS(false);
            Engine.SetVSync(settings.VSync);
            Engine.SetAngleSystem(CONST_TV_ANGLE.TV_ANGLE_DEGREE);
            Engine.SetFPUPrecision(true);
            Engine.SetWatermarkParameters(CONST_TV_WATERMARKPLACE.TV_WATERMARK_BOTTOMRIGHT, 0.5f);
            if (antialias != CONST_TV_MULTISAMPLE_TYPE.TV_MULTISAMPLE_NONE)
            {
                Engine.SetAntialiasing(true, antialias);
            }

            Scene = new TVScene();
            Scene.SetBackgroundColor((float)settings.BackColor.R / 255, (float)settings.BackColor.G / 255, (float)settings.BackColor.B / 255);
            Scene.SetRenderMode(CONST_TV_RENDERMODE.TV_SOLID);
            Scene.SetMipmappingPrecision(0.1f);
            Scene.SetDepthBuffer(CONST_TV_DEPTHBUFFER.TV_ZBUFFER);
            Scene.SetShadeMode(CONST_TV_SHADEMODE.TV_SHADEMODE_PHONG);
            Scene.SetAutoTransColor((int)CONST_TV_COLORKEY.TV_COLORKEY_USE_ALPHA_CHANNEL);
            Scene.SetTextureFilter(CONST_TV_TEXTUREFILTER.TV_FILTER_ANISOTROPIC);

            Physics = new TVPhysics();
            Physics.Initialize();
            Physics.SetSolverModel(CONST_TV_PHYSICS_SOLVER.TV_SOLVER_EXACT);
            Physics.SetFrictionModel(CONST_TV_PHYSICS_FRICTION.TV_FRICTION_EXACT);
            Physics.SetGlobalGravity(new TV_3DVECTOR(0, -9.8f, 0));
            Physics.EnableCPUOptimizations(true);

            Globals = new TVGlobals();
            TextureFactory = new TVTextureFactory();

            Input = new TVInputEngine();
            Input.Initialize(true, true);

            MathLibrary = new TVMathLibrary();

            LightEngine = new TVLightEngine();
            LightEngine.SetGlobalAmbient(0f, 0f, 0f);
            LightEngine.SetSpecularLighting(true);

            MaterialFactory = new TVMaterialFactory();

            GraphicEffect = new TVGraphicEffect();

            Screen2DImmediate = new TVScreen2DImmediate();

            InternalObjects = new TVInternalObjects();

            Text2D = new TVScreen2DText();
            fontID = Text2D.NormalFont_Create("Arial", "Arial", 16, false, false, false);

            Camera = new TVCamera();
            Camera = Scene.GetCamera();
            Camera.SetViewFrustum(45, 30000f, 1f);
            Camera.SetPosition(0, 5, -20);
            Camera.SetLookAt(0, 0, 0);

            Viewport = Engine.GetViewport();
            Viewport.SetCamera(Camera);

            Shader = new TVShader();

            Atmosphere = new TVAtmosphere();
            Atmosphere.SkyBox_Enable(false);
            Atmosphere.SkySphere_Enable(false);

            Engine.AddToLog("Begin initializing sound (irrKlang)");
            try
            {
                SoundFactory = new SoundFactory();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Error initializing sound engine.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Engine.AddToLog("End initializing sound (irrKlang)");


            //DrawGroud();
        }

        public TVScreen2DText Text2D { get; set; }

        #region ICore Members

        public TVEngine Engine { get; private set; }
        public TVScene Scene { get; set; }
        public TVCamera Camera { get; set; }
        public TVGlobals Globals { get; set; }
        public TVInputEngine Input { get; set; }
        public TVViewport Viewport { get; set; }
        public TVMathLibrary MathLibrary { get; set; }
        public TVGraphicEffect GraphicEffect { get; set; }
        public TVTextureFactory TextureFactory { get; set; }
        //public TVRenderSurface RenderSurface { get; set; }
        public TVLightEngine LightEngine { get; set; }
        public TVMaterialFactory MaterialFactory { get; set; }
        public TVScreen2DImmediate Screen2DImmediate { get; set; }
        public TVInternalObjects InternalObjects { get; set; }
        public TVCollisionResult CollisionResult { get; set; }
        public TVShader Shader { get; set; }
        public TVAtmosphere Atmosphere { get; set; }
        public TVPhysics Physics { get; set; }
        public SoundFactory SoundFactory { get; private set; }

        public List<ObjectBase> AllObjects { get; set; }

        #endregion

        ~Core()
        {
            Engine.ReleaseAll();
            Engine = null;
        }

        private void DrawGroud()
        {
            TVLandscape floor = Scene.CreateLandscape();
            floor.CreateEmptyTerrain(CONST_TV_LANDSCAPE_PRECISION.TV_PRECISION_BEST, 1, 1, -128, 0, -128);
            floor.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
            floor.SetTexture(Globals.GetTex("ground"));
            floor.SetTextureScale(100, 100);
        }

        public void PreUpdate()
        {
            // Render water
            foreach (ObjectBase o in AllObjects)
            {
                if (o.GetType() == typeof(Water))
                {
                    (o as Water).ReflectRS.StartRender();
                    //                     foreach (ObjectBase i in AllObjects)
                    //                     {
                    //                         if (i.GetType() != typeof(Water) && i.Renderable)
                    //                             i.GetMesh().Render();
                    //                     }
                    RenderAllExcept(typeof(Water));
                    if (IsSkyBox)
                    {
                        Atmosphere.SkyBox_Render();
                    }
                    else if (IsSkySphere)
                    {
                        Atmosphere.SkySphere_Render();
                    }
                    (o as Water).ReflectRS.EndRender();
                }
            }

            foreach (ObjectBase o in AllObjects)
            {
                if (o.GetType() == typeof(Water))
                {
                    (o as Water).RefractRS.StartRender();
                    //                     foreach (ObjectBase i in AllObjects)
                    //                     {
                    //                         if (i.GetType() != typeof(Water) && i.Renderable)
                    //                             i.GetMesh().Render();
                    //                     }
                    RenderAllExcept(typeof(Water));
                    if (IsSkyBox)
                    {
                        Atmosphere.SkyBox_Render();
                    }
                    else if (IsSkySphere)
                    {
                        Atmosphere.SkySphere_Render();
                    }
                    (o as Water).RefractRS.EndRender();
                }
            }
            //End render water

            Engine.Clear(false);

            if (PreviewingScene)
            {
                try
                {
                    Physics.Simulate(Engine.AccurateTimeElapsed() / 750);
                }
                catch (Exception) { }
            }
        }

        public void Update()
        {
            try
            {
                if (IsSkyBox)
                {
                    Atmosphere.SkyBox_Render();
                }
                else if (IsSkySphere)
                {
                    Atmosphere.SkySphere_Render();
                }

                Scene.RenderAll(true);

                if(settings.Shadows)
                    Scene.FinalizeShadows();

                SoundFactory.Update(Camera);
            }
            catch (Exception) { }
        }

        public void PostUpdate()
        {
            Engine.RenderToScreen();
        }

        private void RenderAllExcept(Type objType)
        {
            foreach (ObjectBase o in AllObjects)
            {
                if (o.Renderable && o.GetType() != objType)
                {
                    o.GetMesh().Render();
                }
            }
        }

        private void RenderOnly(Type objType)
        {
            foreach (ObjectBase o in AllObjects)
            {
                if (o.Renderable && o.GetType() == objType)
                {
                    o.GetMesh().Render();
                }
            }
        }

        public void Debug(string text, int lineNumber)
        {
            Text2D.Action_BeginText();
            int fromTop = 20 * lineNumber;

            Text2D.TextureFont_DrawText(text, 10, fromTop, Globals.Colorkey(CONST_TV_COLORKEY.TV_COLORKEY_WHITE), fontID);
            fromTop += 20;

            Text2D.Action_EndText();
        }

        public T GetObjectByUniqueId<T>(string uniqueId) where T : ObjectBase
        {
            return AllObjects.Find(o => o.UniqueId == uniqueId) as T;
        }

        public ObjectBase GetObjectByUniqueId(string uniqueId)
        {
            return AllObjects.Find(o => o.UniqueId == uniqueId);
        }

        public void ResetErrorMessages()
        {
            errorMessages = new List<string>();
        }

        public IEnumerable<T> GetObjects<T>() where T : ObjectBase
        {
            return AllObjects.FindAll(o => o.GetType().Equals(typeof(T))).Cast<T>();
        } 

        public void SaveScene2(string fileName)
        {
            // List<A> listOfA = new List<C>().Cast<A>().ToList();
            // List<A> listOfA = new List<C>().ConvertAll(x => (A)x);

            var meshObjects = GetObjects<Mesh>();
            var triggers = GetObjects<Trigger>();
            var pointLights = GetObjects<PointLight>();
            var directionalLight = GetObjects<DirectionalLight>();
            var waterPlanes = GetObjects<Water>();
            var skySphere = GetObjects<SkySphere>();
            var skyBox = GetObjects<SkyBox>();
            var sounds = GetObjects<Sound>();
            var particles = GetObjects<Particle>();

            XDocument doc = new XDocument(
                new XElement("Scene",
                    new XElement("Objects",
                        meshObjects.Select(m => new XElement("Object",
                            new XAttribute("Name", m.Name),
                            new XAttribute("FileName", RemoveBasePath(m.FileName)),
                            new XAttribute("IsAnimated", m.IsAnimated),
                            new XAttribute("CustomTexture", RemoveBasePath(m.CustomTexture)),
                            new XAttribute("ScaleTexture", UVToString(m.TextureScale)),
                            new XAttribute("Visible", m.Visible),
                            new XAttribute("Lightning", m.EnableLightning),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Rotation", VECTOR3DToString(m.Rotation)),
                            new XAttribute("Scale", VECTOR3DToString(m.Scale)),
                            new XElement("Physics",
                                new XAttribute("Mass", m.Mass),
                                new XAttribute("Bounding", m.Bounding),
                                new XAttribute("StaticFriction", m.StaticFriction),
                                new XAttribute("KineticFriction", m.KineticFriction),
                                new XAttribute("Softness", m.Softness),
                                new XAttribute("Bounciness", m.Bounciness)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Triggers",
                        triggers.Select(m => new XElement("Trigger",
                            new XAttribute("Name", m.Name),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Rotation", VECTOR3DToString(m.Rotation)),
                            new XAttribute("Scale", VECTOR3DToString(m.Scale)),
                            new XAttribute("Color", MyColorToString(m.Color)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Lights",
                        pointLights.Select(m => new XElement("Point",
                            new XAttribute("Name", m.Name),
                            new XAttribute("Visible", m.Visible),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Radius", m.Radius),
                            new XAttribute("Color", MyColorToString(m.Color)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            )),
                        directionalLight.Select(m => new XElement("Directional",
                            new XAttribute("Name", m.Name),
                            new XAttribute("Visible", m.Visible),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Direction", VECTOR3DToString(m.Direction)),
                            new XAttribute("Color", MyColorToString(m.Color)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("WaterPlanes",
                        waterPlanes.Select(m => new XElement("Water",
                            new XAttribute("Name", m.Name),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Scale", VECTOR3DToString(m.Scale)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Sky",
                        skySphere.Select(m => new XElement("SkySphere",
                            new XAttribute("Name", m.Name),
                            new XAttribute("Rotation", VECTOR3DToString(m.Rotation)),
                            new XAttribute("Scale", VECTOR3DToString(m.Scale)),
                            new XAttribute("Texture", RemoveBasePath(m.Texture)),
                            new XAttribute("PolyCount", m.PolyCount),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            )),
                        skyBox.Select(m => new XElement("SkyBox",
                            new XAttribute("Name", m.Name),
                            new XAttribute("FrontTexture", RemoveBasePath(m.Front)),
                            new XAttribute("BackTexture", RemoveBasePath(m.Back)),
                            new XAttribute("TopTexture", RemoveBasePath(m.Top)),
                            new XAttribute("BottomTexture", RemoveBasePath(m.Bottom)),
                            new XAttribute("LeftTexture", RemoveBasePath(m.Left)),
                            new XAttribute("RightTexture", RemoveBasePath(m.Right)),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Sounds",
                        sounds.Select(m => new XElement("Sound",
                            new XAttribute("Name", m.Name),
                            new XAttribute("FileName", RemoveBasePath(m.FileName)),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Volume", m.Volume),
                            new XAttribute("Stopped", m.Stopped),
                            new XAttribute("Loop", m.Loop),
                            new XAttribute("Is3D", m.Is3D),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Particles",
                        particles.Select(m => new XElement("Particle",
                            new XAttribute("Name", m.Name),
                            new XAttribute("FileName", RemoveBasePath(m.FileName)),
                            new XAttribute("Position", VECTOR3DToString(m.Position)),
                            new XAttribute("Rotation", VECTOR3DToString(m.Rotation)),
                            new XAttribute("Scale", VECTOR3DToString(m.Scale)),
                            new XAttribute("Visible", m.Visible),
                            new XElement(GetParametersData(m)),
                            new XElement(GetScriptData(m))
                            ))),
                    new XElement("Editor",
                            new XAttribute("CamPosition", TV3DVECTORToString(Camera.GetPosition())),
                            new XAttribute("CamLookAt", TV3DVECTORToString(Camera.GetLookAt()))
                            )
                ));

            doc.Save(fileName);
        }

        private XElement GetParametersData(ObjectBase m)
        {
            return new XElement("CustomParameters",
                                m.CustomParams.Cast<Custom>().Select(p => new XElement("Parameter",
                                    new XAttribute("Name", p.Name),
                                    new XAttribute("Value", p.Value))));
        }

        private XElement GetScriptData(ObjectBase m)
        {
            return new XElement("CustomScript",
                                new XAttribute("Enabled", m.ScriptEnabled),
                                new XCData(m.Script)
                                );
        }

        private string UVToString(UV value)
        {
            return string.Format("{0} {1}", value.U, value.V);
        }

        private string MyColorToString(MyColor value)
        {
            return string.Format("{0} {1} {2}", value.R, value.G, value.B);
        }

        private string VECTOR3DToString(VECTOR3D value)
        {
            return string.Format("{0} {1} {2}", value.X, value.Y, value.Z);
        }

        private string TV3DVECTORToString(TV_3DVECTOR value)
        {
            return string.Format("{0} {1} {2}", value.x, value.y, value.z);
        }

        private string VECTOR2DToString(VECTOR2D value)
        {
            return string.Format("{0} {1}", value.X, value.Y);
        }

        private string RemoveBasePath(string value)
        {
            if (value.Equals(string.Empty))
                return string.Empty;
            else
                return value.Replace(Application.StartupPath, string.Empty).Remove(0, 1);
        }

        public void SaveScene(string fileName)
        {
            try
            {
                var xmlDoc = new XmlDocument();

                // Write down the XML declaration
                var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                // Create the root element
                var sceneNode = xmlDoc.CreateElement("Scene");
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(sceneNode);

                var objectsNode = xmlDoc.CreateElement("Objects");
                sceneNode.AppendChild(objectsNode);

                foreach (Mesh obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Mesh))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Object");

                    objectsNode.PrependChild(parentNode);
                    //xmlDoc.DocumentElement.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var fileNameNode = xmlDoc.CreateElement("FileName");
                    var isAnimatedNode = xmlDoc.CreateElement("IsAnimated");
                    var customTextureNode = xmlDoc.CreateElement("CustomTexture");
                    var scaleTextureNode = xmlDoc.CreateElement("ScaleTexture");
                    var visibleNode = xmlDoc.CreateElement("Visible");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var rotationNode = xmlDoc.CreateElement("Rotation");
                    var scaleNode = xmlDoc.CreateElement("Scale");
                    var lightningNone = xmlDoc.CreateElement("Lightning");
                    var massNode = xmlDoc.CreateElement("Mass");
                    var boundingNode = xmlDoc.CreateElement("Bounding");
                    var staticFrictionNode = xmlDoc.CreateElement("StaticFriction");
                    var kineticFrictionNode = xmlDoc.CreateElement("KineticFriction");
                    var softnessNode = xmlDoc.CreateElement("Softness");
                    var bouncinessNode = xmlDoc.CreateElement("Bounciness");

                    // retrieve the values
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var fileNameValue =
                        xmlDoc.CreateTextNode(obj.FileName.Replace(Application.StartupPath, string.Empty).Remove(0, 1));
                    var isAnimatedValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.IsAnimated == true ? "true" : "false"));
                    var customTextureValue = xmlDoc.CreateTextNode(string.Empty);
                    if (obj.CustomTexture != string.Empty)
                        customTextureValue = xmlDoc.CreateTextNode(obj.CustomTexture.Replace(Application.StartupPath, string.Empty).Remove(0, 1));
                    var scaleTextureValue = xmlDoc.CreateTextNode(string.Format("{0} {1}", obj.TextureScale.U, obj.TextureScale.V));
                    var visibleValue = xmlDoc.CreateTextNode(obj.Visible ? "true" : "false");
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var rotationValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Rotation.X, obj.Rotation.Y,
                                                                                obj.Rotation.Z));
                    var scaleValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                             obj.Scale.X, obj.Scale.Y, obj.Scale.Z));
                    var massValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.Mass));
                    var lightningValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.EnableLightning == true ? "true" : "false"));
                    var boundingValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.Bounding.ToString()));
                    var staticFrictionValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.StaticFriction.ToString()));
                    var kineticFrictionValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.KineticFriction.ToString()));
                    var softnessValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.Softness.ToString()));
                    var bouncinessValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.Bounciness.ToString()));

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(fileNameNode);
                    parentNode.AppendChild(isAnimatedNode);
                    parentNode.AppendChild(customTextureNode);
                    parentNode.AppendChild(scaleTextureNode);
                    parentNode.AppendChild(visibleNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(rotationNode);
                    parentNode.AppendChild(scaleNode);
                    parentNode.AppendChild(lightningNone);
                    parentNode.AppendChild(massNode);
                    parentNode.AppendChild(boundingNode);
                    parentNode.AppendChild(staticFrictionNode);
                    parentNode.AppendChild(kineticFrictionNode);
                    parentNode.AppendChild(softnessNode);
                    parentNode.AppendChild(bouncinessNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    fileNameNode.AppendChild(fileNameValue);
                    isAnimatedNode.AppendChild(isAnimatedValue);
                    customTextureNode.AppendChild(customTextureValue);
                    scaleTextureNode.AppendChild(scaleTextureValue);
                    visibleNode.AppendChild(visibleValue);
                    positionNode.AppendChild(positionValue);
                    rotationNode.AppendChild(rotationValue);
                    scaleNode.AppendChild(scaleValue);
                    lightningNone.AppendChild(lightningValue);
                    massNode.AppendChild(massValue);
                    boundingNode.AppendChild(boundingValue);
                    staticFrictionNode.AppendChild(staticFrictionValue);
                    kineticFrictionNode.AppendChild(kineticFrictionValue);
                    softnessNode.AppendChild(softnessValue);
                    bouncinessNode.AppendChild(bouncinessValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var lightsNode = xmlDoc.CreateElement("Lights");
                sceneNode.AppendChild(lightsNode);

                foreach (DirectionalLight obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(DirectionalLight))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Directional");

                    lightsNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var visibleNode = xmlDoc.CreateElement("Visible");
                    var positionNode = xmlDoc.CreateElement("Position");
                    positionNode.SetAttribute("comment", "Position value used in editor.");
                    var directionNode = xmlDoc.CreateElement("Direction");
                    var colorNode = xmlDoc.CreateElement("Color");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var visibleValue = xmlDoc.CreateTextNode(obj.Visible ? "true" : "false");
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var directionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Direction.X, obj.Direction.Y,
                                                                                obj.Direction.Z));
                    var colorValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                             obj.Color.R, obj.Color.G, obj.Color.B));

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(visibleNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(directionNode);
                    parentNode.AppendChild(colorNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    visibleNode.AppendChild(visibleValue);
                    positionNode.AppendChild(positionValue);
                    directionNode.AppendChild(directionValue);
                    colorNode.AppendChild(colorValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                foreach (PointLight obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(PointLight))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Point");

                    lightsNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var visibleNode = xmlDoc.CreateElement("Visible");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var radiusNode = xmlDoc.CreateElement("Radius");
                    var colorNode = xmlDoc.CreateElement("Color");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var visibleValue = xmlDoc.CreateTextNode(obj.Visible ? "true" : "false");
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var radiusValue = xmlDoc.CreateTextNode(string.Format("{0}", obj.Radius));
                    var colorValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                             obj.Color.R, obj.Color.G, obj.Color.B));

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(visibleNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(radiusNode);
                    parentNode.AppendChild(colorNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    visibleNode.AppendChild(visibleValue);
                    positionNode.AppendChild(positionValue);
                    radiusNode.AppendChild(radiusValue);
                    colorNode.AppendChild(colorValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var triggersNode = xmlDoc.CreateElement("Triggers");
                sceneNode.AppendChild(triggersNode);

                foreach (Trigger obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Trigger))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Trigger");

                    triggersNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var rotationNode = xmlDoc.CreateElement("Rotation");
                    var scaleNode = xmlDoc.CreateElement("Scale");
                    var colorNode = xmlDoc.CreateElement("Color");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var rotationValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Rotation.X, obj.Rotation.Y,
                                                                                obj.Rotation.Z));
                    var scaleValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                               obj.Scale.X, obj.Scale.Y,
                                                                               obj.Scale.Z));
                    var colorValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                             obj.Color.R, obj.Color.G, obj.Color.B));

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(rotationNode);
                    parentNode.AppendChild(scaleNode);
                    parentNode.AppendChild(colorNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    positionNode.AppendChild(positionValue);
                    rotationNode.AppendChild(rotationValue);
                    scaleNode.AppendChild(scaleValue);
                    colorNode.AppendChild(colorValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var soundsNode = xmlDoc.CreateElement("Sounds");
                sceneNode.AppendChild(soundsNode);

                foreach (Sound obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Sound))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Sound");

                    soundsNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var fileNameNode = xmlDoc.CreateElement("FileName");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var volumeNode = xmlDoc.CreateElement("Volume");
                    var stoppedNode = xmlDoc.CreateElement("Stopped");
                    var loopNode = xmlDoc.CreateElement("Loop");
                    var is3DNode = xmlDoc.CreateElement("Is3D");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var fileNameValue =
                        xmlDoc.CreateTextNode(obj.FileName.Replace(Application.StartupPath, string.Empty).Remove(0, 1));
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var volumeValue = xmlDoc.CreateTextNode(((float)obj.Volume / 100).ToString());
                    var stoppedValue = xmlDoc.CreateTextNode(obj.Stopped == true ? "true" : "false");
                    var loopValue = xmlDoc.CreateTextNode(obj.Loop == true ? "true" : "false");
                    var is3DValue = xmlDoc.CreateTextNode(obj.Is3D == true ? "true" : "false");

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(fileNameNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(volumeNode);
                    parentNode.AppendChild(stoppedNode);
                    parentNode.AppendChild(loopNode);
                    parentNode.AppendChild(is3DNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    fileNameNode.AppendChild(fileNameValue);
                    positionNode.AppendChild(positionValue);
                    volumeNode.AppendChild(volumeValue);
                    stoppedNode.AppendChild(stoppedValue);
                    loopNode.AppendChild(loopValue);
                    is3DNode.AppendChild(is3DValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var particlesNode = xmlDoc.CreateElement("Particles");
                sceneNode.AppendChild(particlesNode);

                foreach (Particle obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Particle))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Particle");

                    particlesNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var fileNameNode = xmlDoc.CreateElement("FileName");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var rotationNode = xmlDoc.CreateElement("Rotation");
                    var scaleNode = xmlDoc.CreateElement("Scale");
                    var visbleNode = xmlDoc.CreateElement("Visible");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var fileNameValue =
                        xmlDoc.CreateTextNode(obj.FileName.Replace(Application.StartupPath, string.Empty).Remove(0, 1));
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var rotationValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Rotation.X, obj.Rotation.Y,
                                                                                obj.Rotation.Z));
                    var scaleValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Scale.X, obj.Scale.Y,
                                                                                obj.Scale.Z));
                    var visbleValue = xmlDoc.CreateTextNode(obj.Visible == true ? "true" : "false");

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(fileNameNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(rotationNode);
                    parentNode.AppendChild(scaleNode);
                    parentNode.AppendChild(visbleNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    fileNameNode.AppendChild(fileNameValue);
                    positionNode.AppendChild(positionValue);
                    rotationNode.AppendChild(rotationValue);
                    scaleNode.AppendChild(scaleValue);
                    visbleNode.AppendChild(visbleValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var waterPlanesNode = xmlDoc.CreateElement("WaterPlanes");
                sceneNode.AppendChild(waterPlanesNode);

                foreach (Water obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Water))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("Water");

                    waterPlanesNode.PrependChild(parentNode);

                    // Create the required nodes
                    var nameNode = xmlDoc.CreateElement("Name");
                    var positionNode = xmlDoc.CreateElement("Position");
                    var scaleNode = xmlDoc.CreateElement("Scale");

                    // retrieve the values 
                    var nameValue = xmlDoc.CreateTextNode(obj.Name);
                    var positionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Position.X, obj.Position.Y,
                                                                                obj.Position.Z));
                    var scaleValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                               obj.Scale.X, obj.Scale.Y,
                                                                               obj.Scale.Z));

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(nameNode);
                    parentNode.AppendChild(positionNode);
                    parentNode.AppendChild(scaleNode);

                    // save the value of the fields into the nodes
                    nameNode.AppendChild(nameValue);
                    positionNode.AppendChild(positionValue);
                    scaleNode.AppendChild(scaleValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                var skyNode = xmlDoc.CreateElement("Sky");
                sceneNode.AppendChild(skyNode);

                foreach (SkySphere obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(SkySphere))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("SkySphere");

                    skyNode.PrependChild(parentNode);

                    // Create the required nodes
                    var rotationNode = xmlDoc.CreateElement("Rotation");
                    var scaleNode = xmlDoc.CreateElement("Scale");
                    var textureNode = xmlDoc.CreateElement("Texture");
                    var polyCountNode = xmlDoc.CreateElement("PolyCount");

                    // retrieve the values 
                    var textureValue = xmlDoc.CreateTextNode(obj.FileName);

                    // append the nodes to the parentNode without the value
                    var rotationValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Rotation.X, obj.Rotation.Y,
                                                                                obj.Rotation.Z));
                    var scaleValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                                obj.Scale.X, obj.Scale.Y,
                                                                                obj.Scale.Z));

                    var polyCountValue = xmlDoc.CreateTextNode(obj.PolyCount.ToString());

                    parentNode.AppendChild(rotationNode);
                    parentNode.AppendChild(scaleNode);
                    parentNode.AppendChild(textureNode);
                    parentNode.AppendChild(polyCountNode);

                    // save the value of the fields into the nodes
                    rotationNode.AppendChild(rotationValue);
                    scaleNode.AppendChild(scaleValue);
                    textureNode.AppendChild(textureValue);
                    polyCountNode.AppendChild(polyCountValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                foreach (SkyBox obj in AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(SkyBox))))
                {
                    // Create a new <Object> element and add it to the root node
                    var parentNode = xmlDoc.CreateElement("SkyBox");

                    skyNode.PrependChild(parentNode);

                    // Create the required nodes
                    var frontTextureNode = xmlDoc.CreateElement("FrontTexture");
                    var backTextureNode = xmlDoc.CreateElement("BackTexture");
                    var leftTextureNode = xmlDoc.CreateElement("LeftTexture");
                    var rightTextureNode = xmlDoc.CreateElement("RightTexture");
                    var topTextureNode = xmlDoc.CreateElement("TopTexture");
                    var bottomTextureNode = xmlDoc.CreateElement("BottomTexture");

                    // retrieve the values 
                    var frontTextureValue = xmlDoc.CreateTextNode(obj.FrontFileName);
                    var backTextureValue = xmlDoc.CreateTextNode(obj.BackFileName);
                    var leftTextureValue = xmlDoc.CreateTextNode(obj.LeftFileName);
                    var rightTextureValue = xmlDoc.CreateTextNode(obj.RightFileName);
                    var topTextureValue = xmlDoc.CreateTextNode(obj.TopFileName);
                    var bottomTextureValue = xmlDoc.CreateTextNode(obj.BottomFileName);

                    // append the nodes to the parentNode without the value
                    parentNode.AppendChild(frontTextureNode);
                    parentNode.AppendChild(backTextureNode);
                    parentNode.AppendChild(leftTextureNode);
                    parentNode.AppendChild(rightTextureNode);
                    parentNode.AppendChild(topTextureNode);
                    parentNode.AppendChild(bottomTextureNode);

                    // save the value of the fields into the nodes
                    frontTextureNode.AppendChild(frontTextureValue);
                    backTextureNode.AppendChild(backTextureValue);
                    leftTextureNode.AppendChild(leftTextureValue);
                    rightTextureNode.AppendChild(rightTextureValue);
                    topTextureNode.AppendChild(topTextureValue);
                    bottomTextureNode.AppendChild(bottomTextureValue);

                    SaveCustomParameters(xmlDoc, parentNode, obj);
                    SaveCustomScript(xmlDoc, parentNode, obj);
                }

                // Editor options
                XmlElement editorNode = xmlDoc.CreateElement("Editor");
                sceneNode.AppendChild(editorNode);
                //Camera position
                XmlElement camPositionNode = xmlDoc.CreateElement("CamPosition");
                XmlText camPositionValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                               Camera.GetPosition().x,
                                                                               Camera.GetPosition().y,
                                                                               Camera.GetPosition().z));
                editorNode.AppendChild(camPositionNode);
                camPositionNode.AppendChild(camPositionValue);
                // Camera look at
                XmlElement camLookAtNode = xmlDoc.CreateElement("CamLookAt");
                XmlText camLookAtValue = xmlDoc.CreateTextNode(string.Format("{0} {1} {2}",
                                                                             Camera.GetLookAt().x, Camera.GetLookAt().y,
                                                                             Camera.GetLookAt().z));
                editorNode.AppendChild(camLookAtNode);
                camLookAtNode.AppendChild(camLookAtValue);

                // Save to the XML file
                xmlDoc.Save(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveCustomParameters(XmlDocument xmlDoc, XmlElement parentNode, ObjectBase obj)
        {
            var customParentNode = xmlDoc.CreateElement("CustomParameters");
            customParentNode.InnerText = "";

            for (int i = 0; i < obj.CustomParams.Count; i++)
            {
                var customNode = xmlDoc.CreateElement("Parameter");
                var customNameNode = xmlDoc.CreateElement("Name");
                //var customTypeNode = xmlDoc.CreateElement("Type");
                var customValueNode = xmlDoc.CreateElement("Value");
                var customNameValue = xmlDoc.CreateTextNode(obj.CustomParams[i].Name);
                //var customTypeValue = xmlDoc.CreateTextNode(Helpers.DataTypeToString(obj.CustomParams[i].DataType));
                var customValueValue = xmlDoc.CreateTextNode(obj.CustomParams[i].Value);
                customNode.AppendChild(customNameNode);
                //customNode.AppendChild(customTypeNode);
                customNode.AppendChild(customValueNode);
                customNameNode.AppendChild(customNameValue);
                //customTypeNode.AppendChild(customTypeValue);
                customValueNode.AppendChild(customValueValue);

                customParentNode.AppendChild(customNode);
            }

            parentNode.AppendChild(customParentNode);
        }

        private void SaveCustomScript(XmlDocument xmlDoc, XmlElement parentNode, ObjectBase obj)
        {
            var customScriptNode = xmlDoc.CreateElement("CustomScript");
            customScriptNode.InnerText = "";

            var enabledNode = xmlDoc.CreateElement("Enabled");
            var scriptNode = xmlDoc.CreateElement("Script");
            var enabledNodeValue = xmlDoc.CreateTextNode(obj.ScriptEnabled == true ? "true" : "false");
            var scriptNodeValue = xmlDoc.CreateCDataSection(obj.Script);
            enabledNode.AppendChild(enabledNodeValue);
            scriptNode.AppendChild(scriptNodeValue);
            customScriptNode.AppendChild(enabledNode);
            customScriptNode.AppendChild(scriptNode);

            parentNode.AppendChild(customScriptNode);
        }

        public void LoadScene(string fileName)
        {
            LoadingScene = true;
            try
            {
                ResetErrorMessages();

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                int pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Objects/*"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var file = objectNode.SelectSingleNode("FileName").FirstChild.Value;
                    var isAnimated = GetBool(objectNode.SelectSingleNode("IsAnimated").FirstChild.Value);
                    string customTexture = string.Empty;
                    if (objectNode.SelectSingleNode("CustomTexture").FirstChild != null)
                        customTexture = objectNode.SelectSingleNode("CustomTexture").FirstChild.Value;
                    var visible = false;
                    if (objectNode.SelectSingleNode("Visible").FirstChild.Value.Equals("true"))
                        visible = true;
                    var scaleTexture = GetUV(objectNode.SelectSingleNode("ScaleTexture").FirstChild.Value);
                    var position = GetVECTOR3D(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var rotation = GetVECTOR3D(objectNode.SelectSingleNode("Rotation").FirstChild.Value);
                    var scale = GetVECTOR3D(objectNode.SelectSingleNode("Scale").FirstChild.Value);
                    var lightning = GetBool(objectNode.SelectSingleNode("Lightning").FirstChild.Value);
                    var mass = GetFloat(objectNode.SelectSingleNode("Mass").FirstChild.Value);
                    var staticFriction = GetFloat(objectNode.SelectSingleNode("StaticFriction").FirstChild.Value);
                    var kineticFriction = GetFloat(objectNode.SelectSingleNode("KineticFriction").FirstChild.Value);
                    var softness = GetFloat(objectNode.SelectSingleNode("Softness").FirstChild.Value);
                    var bounciness = GetFloat(objectNode.SelectSingleNode("Bounciness").FirstChild.Value);

                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Objects/Object[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Objects/Object[{0}]/CustomScript/*", pozId));

                    Engine.Helpers.BodyBounding bounding;
                    switch (objectNode.SelectSingleNode("Bounding").FirstChild.Value.ToUpper())
                    {
                        case "BOX":
                            bounding = Helpers.BodyBounding.Box;
                            break;
                        case "CONVEXHULL":
                            bounding = Helpers.BodyBounding.Convexhull;
                            break;
                        case "CYLINDER":
                            bounding = Helpers.BodyBounding.Cylinder;
                            break;
                        case "NONE":
                            bounding = Helpers.BodyBounding.None;
                            break;
                        case "SPHERE":
                            bounding = Helpers.BodyBounding.Sphere;
                            break;
                        default:
                            bounding = Helpers.BodyBounding.Convexhull;
                            break;
                    }

                    var type = Helpers.GetObjectType(file);
                    var fileFullPath = Path.Combine(Application.StartupPath, file);
                    string customTextureFullPath = string.Empty;
                    if (!customTexture.Equals(string.Empty))
                        customTextureFullPath = Path.Combine(Application.StartupPath, customTexture);

                    if (!File.Exists(fileFullPath))
                    {
                        if (!errorMessages.Contains(fileFullPath))
                        {
                            string message = string.Format("File doesn't exists {0}", fileFullPath);
                            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errorMessages.Add(fileFullPath);
                            Engine.AddToLog(string.Format("          ERROR : {0}", message));
                        }

                        continue;
                    }

                    switch (type)
                    {
                        case Helpers.ObjectType.StaticMesh:
                            var mesh = new Mesh(this, settings, fileFullPath)
                                           {
                                               Name = name,
                                               IsAnimated = isAnimated,
                                               CustomTexture = customTextureFullPath,
                                               TextureScale = scaleTexture,
                                               Visible = visible,
                                               Position = position,
                                               Rotation = rotation,
                                               Scale = scale,
                                               EnableLightning = lightning,
                                               Mass = mass,
                                               Bounding = bounding,
                                               StaticFriction = staticFriction,
                                               KineticFriction = kineticFriction,
                                               Softness = softness,
                                               Bounciness = bounciness,
                                               ScriptEnabled = customScript.Enabled,
                                               Script = customScript.Script
                                           };
                            mesh.SetCustomCollection(customParameters);
                            mesh.Update();
                            mesh.UpdatePhysics();
                            break;
                    }
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Lights/Directional"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var visible = false;
                    if (objectNode.SelectSingleNode("Visible").FirstChild.Value.Equals("true"))
                        visible = true;
                    var position = GetTV_3DVECTOR(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var color = GetColor(objectNode.SelectSingleNode("Color").FirstChild.Value);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Lights/Directional[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Lights/Directional[{0}]/CustomScript/*", pozId));

                    var directionalLight = new DirectionalLight(this, position)
                    {
                        Name = name,
                        Visible = visible,
                        Color = new MyColor(color.R, color.G, color.B),
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    directionalLight.SetCustomCollection(customParameters);
                    directionalLight.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Lights/Point"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var visible = false;
                    if (objectNode.SelectSingleNode("Visible").FirstChild.Value.Equals("true"))
                        visible = true;
                    var position = GetTV_3DVECTOR(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var radius = GetFloat(objectNode.SelectSingleNode("Radius").FirstChild.Value);
                    var color = GetColor(objectNode.SelectSingleNode("Color").FirstChild.Value);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Lights/Point[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Lights/Point[{0}]/CustomScript/*", pozId));

                    var pointLight = new PointLight(this, position)
                    {
                        Name = name,
                        Visible = visible,
                        Radius = radius,
                        Color = new MyColor(color.R, color.G, color.B),
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    pointLight.SetCustomCollection(customParameters);
                    pointLight.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Triggers/Trigger"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var position = GetVECTOR3D(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var rotation = GetVECTOR3D(objectNode.SelectSingleNode("Rotation").FirstChild.Value);
                    var scale = GetVECTOR3D(objectNode.SelectSingleNode("Scale").FirstChild.Value);
                    var color = GetColor(objectNode.SelectSingleNode("Color").FirstChild.Value);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Triggers/Trigger[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Triggers/Trigger[{0}]/CustomScript/*", pozId));

                    var trigger = new Trigger(this)
                    {
                        Name = name,
                        Position = position,
                        Rotation = rotation,
                        Scale = scale,
                        Color = new MyColor(color.R, color.G, color.B),
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    trigger.SetCustomCollection(customParameters);
                    trigger.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Sounds/*"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var file = objectNode.SelectSingleNode("FileName").FirstChild.Value;
                    var position = GetVECTOR3D(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    float vol = float.Parse(objectNode.SelectSingleNode("Volume").FirstChild.Value);
                    int volume = (int)Math.Round(vol * 100, 0);
                    bool stopped = objectNode.SelectSingleNode("Stopped").FirstChild.Value.ToLower() == "true" ? true : false;
                    bool loop = objectNode.SelectSingleNode("Loop").FirstChild.Value.ToLower() == "true" ? true : false;
                    bool is3D = objectNode.SelectSingleNode("Is3D").FirstChild.Value.ToLower() == "true" ? true : false;
                    var fullPath = Path.Combine(Application.StartupPath, file);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Sounds/Sound[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Sounds/Sound[{0}]/CustomScript/*", pozId));

                    if (!File.Exists(fullPath))
                    {
                        MessageBox.Show(string.Format("File doesn't exists {0}", fullPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    var sound = new Sound(this, fullPath)
                    {
                        Name = name,
                        Position = position,
                        Volume = volume,
                        Stopped = stopped,
                        Loop = loop,
                        Is3D = is3D,
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    sound.SetCustomCollection(customParameters);
                    sound.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Particles/*"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var file = objectNode.SelectSingleNode("FileName").FirstChild.Value;
                    var position = GetVECTOR3D(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var rotation = GetVECTOR3D(objectNode.SelectSingleNode("Rotation").FirstChild.Value);
                    var scale = GetVECTOR3D(objectNode.SelectSingleNode("Scale").FirstChild.Value);
                    bool visible = objectNode.SelectSingleNode("Visible").FirstChild.Value.ToLower() == "true" ? true : false;
                    var fullPath = Path.Combine(Application.StartupPath, file);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Particles/Particle[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Particles/Particle[{0}]/CustomScript/*", pozId));

                    if (!File.Exists(fullPath))
                    {
                        MessageBox.Show(string.Format("File doesn't exists {0}", fullPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    var particle = new Particle(this, fullPath)
                    {
                        Name = name,
                        Position = position,
                        Rotation = rotation,
                        Scale = scale,
                        Visible = visible,
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    particle.SetCustomCollection(customParameters);
                    particle.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/WaterPlanes/*"))
                {
                    var name = objectNode.SelectSingleNode("Name").FirstChild.Value;
                    var position = GetVECTOR3D(objectNode.SelectSingleNode("Position").FirstChild.Value);
                    var scale = GetVECTOR3D(objectNode.SelectSingleNode("Scale").FirstChild.Value);
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/WaterPlanes/Water[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/WaterPlanes/Water[{0}]/CustomScript/*", pozId));

                    var water = new Water(this)
                    {
                        Name = name,
                        Position = position,
                        Scale = scale,
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    water.SetCustomCollection(customParameters);
                    water.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Sky/SkySphere"))
                {
                    string file = string.Empty;
                    string fullPath = string.Empty;

                    var rotation = GetVECTOR3D(objectNode.SelectSingleNode("Rotation").FirstChild.Value);
                    var scale = GetVECTOR3D(objectNode.SelectSingleNode("Scale").FirstChild.Value);
                    var polyCount = int.Parse(objectNode.SelectSingleNode("PolyCount").FirstChild.Value);

                    if (objectNode.SelectSingleNode("Texture").FirstChild != null)
                    {
                        file = objectNode.SelectSingleNode("Texture").FirstChild.Value;
                        fullPath = Path.Combine(Application.StartupPath, file);
                    }
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Sky/SkySphere[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Sky/SkySphere[{0}]/CustomScript/*", pozId));

                    var sky = new SkySphere(this)
                    {
                        Rotation = rotation,
                        Scale = scale,
                        PolyCount = polyCount,
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    if (!fullPath.Equals(string.Empty))
                    {
                        sky.Texture = fullPath;
                    }
                    sky.SetCustomCollection(customParameters);
                    sky.Update();
                }

                pozId = 0;
                foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes("/Scene/Sky/SkyBox"))
                {
                    string frontFile = string.Empty;
                    string backFile = string.Empty;
                    string leftFile = string.Empty;
                    string rightFile = string.Empty;
                    string topFile = string.Empty;
                    string bottomFile = string.Empty;
                    string frontFullPath = string.Empty;
                    string backFullPath = string.Empty;
                    string leftFullPath = string.Empty;
                    string rightFullPath = string.Empty;
                    string topFullPath = string.Empty;
                    string bottomFullPath = string.Empty;

                    if (objectNode.SelectSingleNode("FrontTexture").FirstChild != null)
                    {
                        frontFile = objectNode.SelectSingleNode("FrontTexture").FirstChild.Value;
                        frontFullPath = Path.Combine(Application.StartupPath, frontFile);
                    }

                    if (objectNode.SelectSingleNode("BackTexture").FirstChild != null)
                    {
                        backFile = objectNode.SelectSingleNode("BackTexture").FirstChild.Value;
                        backFullPath = Path.Combine(Application.StartupPath, backFile);
                    }

                    if (objectNode.SelectSingleNode("LeftTexture").FirstChild != null)
                    {
                        leftFile = objectNode.SelectSingleNode("LeftTexture").FirstChild.Value;
                        leftFullPath = Path.Combine(Application.StartupPath, leftFile);
                    }

                    if (objectNode.SelectSingleNode("RightTexture").FirstChild != null)
                    {
                        rightFile = objectNode.SelectSingleNode("RightTexture").FirstChild.Value;
                        rightFullPath = Path.Combine(Application.StartupPath, rightFile);
                    }

                    if (objectNode.SelectSingleNode("TopTexture").FirstChild != null)
                    {
                        topFile = objectNode.SelectSingleNode("TopTexture").FirstChild.Value;
                        topFullPath = Path.Combine(Application.StartupPath, topFile);
                    }

                    if (objectNode.SelectSingleNode("BottomTexture").FirstChild != null)
                    {
                        bottomFile = objectNode.SelectSingleNode("BottomTexture").FirstChild.Value;
                        bottomFullPath = Path.Combine(Application.StartupPath, bottomFile);
                    }
                    var customParameters = LoadCustomParameters(xmlDoc, string.Format("/Scene/Sky/SkyBox[{0}]/CustomParameters/*", ++pozId));
                    var customScript = LoadCustomScript(xmlDoc, string.Format("/Scene/Sky/SkyBox[{0}]/CustomScript/*", pozId));

                    var sky = new SkyBox(this)
                    {
                        ScriptEnabled = customScript.Enabled,
                        Script = customScript.Script
                    };
                    if (!frontFullPath.Equals(string.Empty))
                        sky.Front = frontFullPath;
                    if (!backFullPath.Equals(string.Empty))
                        sky.Back = backFullPath;
                    if (!leftFullPath.Equals(string.Empty))
                        sky.Left = leftFullPath;
                    if (!rightFullPath.Equals(string.Empty))
                        sky.Right = rightFullPath;
                    if (!topFullPath.Equals(string.Empty))
                        sky.Top = topFullPath;
                    if (!bottomFullPath.Equals(string.Empty))
                        sky.Bottom = bottomFullPath;
                    sky.SetCustomCollection(customParameters);
                    sky.Update();
                }

                var camPosition =
                    GetTV_3DVECTOR(xmlDoc.DocumentElement.SelectSingleNode("/Scene/Editor/CamPosition").FirstChild.Value);
                var camLookAt =
                    GetTV_3DVECTOR(xmlDoc.DocumentElement.SelectSingleNode("/Scene/Editor/CamLookAt").FirstChild.Value);

                Camera.SetPosition(camPosition.x, camPosition.y, camPosition.z);
                Camera.SetLookAt(camLookAt.x, camLookAt.y, camLookAt.z);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadingScene = false;
        }

        private CustomCollection LoadCustomParameters(XmlDocument xmlDoc, string xpath)
        {
            var cc = new CustomCollection();

            foreach (XmlNode objectNode in xmlDoc.DocumentElement.SelectNodes(xpath))
            {
                var name = string.Empty;
                //var type = DataTypes.String;
                var value = string.Empty;

                var paramName = objectNode.SelectSingleNode("Name").FirstChild;
                var paramValue = objectNode.SelectSingleNode("Value").FirstChild;

                if (paramName != null)
                {
                    if (paramName.Value != null)
                    {
                        name = paramName.Value;
                    }
                }
                //if (objectNode.SelectSingleNode("Type").FirstChild.Value != null)
                //type = Helpers.DataTypeFromString(objectNode.SelectSingleNode("Type").FirstChild.Value);
                if (paramValue != null)
                {
                    if (paramValue.Value != null)
                    {
                        value = paramValue.Value;
                    }
                }
                cc.Add(new Custom(name, /*type,*/ value));
            }

            return cc;
        }

        private ScriptInfo LoadCustomScript(XmlDocument xmlDoc, string xpath)
        {
            var info = new ScriptInfo();
            XmlNode enabledNode = xmlDoc.DocumentElement.SelectNodes(xpath).Item(0);
            XmlNode scriptNode = xmlDoc.DocumentElement.SelectNodes(xpath).Item(1);

            info.Enabled = enabledNode.FirstChild.Value == "true" ? true : false;
            info.Script = scriptNode.FirstChild.InnerText;

            return info;
        }

        private Color GetColor(string value)
        {
            var result = new Color();
            var v = value.Split(' ');

            byte r = 0;
            byte.TryParse(v[0], out r);

            byte g = 0;
            byte.TryParse(v[1], out g);

            byte b = 0;
            byte.TryParse(v[2], out b);

            result = Color.FromArgb(r, g, b);

            return result;
        }

        private VECTOR3D GetVECTOR3D(string value)
        {
            var result = new VECTOR3D();
            string[] v = value.Split(' ');

            float f = 0f;
            float.TryParse(v[0], out f);
            result.X = f;

            f = 0f;
            float.TryParse(v[1], out f);
            result.Y = f;

            f = 0f;
            float.TryParse(v[2], out f);
            result.Z = f;

            return result;
        }

        private VECTOR2D GetVECTOR2D(string value)
        {
            var result = new VECTOR2D();
            string[] v = value.Split(' ');

            float f = 0f;
            float.TryParse(v[0], out f);
            result.X = f;

            f = 0f;
            float.TryParse(v[1], out f);
            result.Y = f;

            return result;
        }

        private UV GetUV(string value)
        {
            var result = new UV();
            string[] v = value.Split(' ');

            float f = 0f;
            float.TryParse(v[0], out f);
            result.U = f;

            f = 0f;
            float.TryParse(v[1], out f);
            result.V = f;

            return result;
        }

        private TV_3DVECTOR GetTV_3DVECTOR(string value)
        {
            VECTOR3D result = GetVECTOR3D(value);
            return new TV_3DVECTOR(result.X, result.Y, result.Z);
        }

        private float GetFloat(string value)
        {
            float result = 0f;
            float.TryParse(value, out result);
            return result;
        }

        private bool GetBool(string value)
        {
            bool result = value.Trim().ToLower() == "true" ? true : false;
            return result;
        }

        public string GetName<T>() where T : ObjectBase
        {
            var typeName = typeof(T).Name;
            var objCount = AllObjects.FindAll(o => o.GetType().Equals(typeof(T))).Count;
            var name = typeName + objCount.ToString();

            // Set another name if object with such name exits
            while (AllObjects.FindAll(o => o.Name != null && o.Name.Equals(name)).Count > 0)
            {
                name = typeName + (++objCount).ToString();
            }

            return name;
        }

        public ProgramSettings Settings
        {
            get { return settings; }
        }

        #region ICore Members


        public bool LoadingScene
        {
            get;
            set;
        }

        #endregion
    }
}