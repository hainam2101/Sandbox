using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Engine;
using Microsoft.DirectX.Direct3D;
using MTV3D65;
using Sandbox.Properties;

namespace Sandbox
{
    public partial class FrmMain : Form
    {
        private Core core;
        private bool creatingObjectCopy = false;
        private string debugText = string.Empty;
        private string defaultProductName;
        private bool doLoop = true;
        private TV_3DVECTOR initialPosition;
        private bool mouseOver = false;
        private TV_3DVECTOR mousePosition3D;
        private Point oldMousePosition;
        private string sceneFileName;
        private ObjectBase selectedObject;
        private TreeNode sceneSkyNode;
        private TreeNode sceneObjectsNode;
        private TreeNode sceneLightsNode;
        private TreeNode sceneSoundsNode;
        private TreeNode sceneWaterNode;
        private TreeNode sceneLandscapeNode;
        private TreeNode sceneTriggersNode;
        private TreeNode sceneParticlesNode;
        private List<Engine.Mesh> allMeshes;

        private Tool selectedTool;
        public ProgramSettings Settings;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            doLoop = false;
            Helpers.SaveSettings(Settings);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            this.Text = string.Format("{0} {1}", Helpers.PROGRAM_NAME, Helpers.PROGRAM_VERSION);
            Helpers.CreateDataPaths(Application.StartupPath);

            Settings = Helpers.LoadSettings();
            LoadObjectInObjectsTreeView(Path.Combine(Application.StartupPath, Settings.FolderBase));
            InitialiseSceneTreeView();

            core = new Core(pnlRenderer.Handle, Settings);
            selectedTool = Tool.None;
            mousePosition3D = core.Globals.Vector3(0, 0, 0);
            initialPosition = core.Globals.Vector3(0, 0, 0);
            sceneFileName = string.Empty;
            defaultProductName = Text;
            btnSnap.Checked = Settings.SnapToGrid;
            allMeshes = new List<Engine.Mesh>();

            Show();
            Focus();
            doLoop = true;

            MainLoop();
        }

        private void LoadObjectInObjectsTreeView(string projectPath)
        {
            tvObjects.Nodes.Clear();
            // Add lights to common objects.
            var node = new TreeNode("Common");
            // Sky
            var skyNode = new TreeNode("Sky");
            // Skybox
            var skyboxNode = new TreeNode("Skybox");
            skyboxNode.ImageIndex = 7;
            skyboxNode.SelectedImageIndex = 7;
            // Skysphere
            var skysphereNode = new TreeNode("SkySphere");
            skysphereNode.ImageIndex = 6;
            skysphereNode.SelectedImageIndex = 6;
            // Water
            var waterBaseNode = new TreeNode("Water");
            var waterNode = new TreeNode("Water");
            waterNode.ImageIndex = 8;
            waterNode.SelectedImageIndex = 8;
            // Lights
            var lightsNode = new TreeNode("Lights");
            // Directional light.
            var directionalNode = new TreeNode("Directional");
            directionalNode.ImageIndex = 2;
            directionalNode.SelectedImageIndex = 2;
            // Point light.
            var pointNode = new TreeNode("Point");
            pointNode.ImageIndex = 3;
            pointNode.SelectedImageIndex = 3;
            // Spot light.
            //var spotNode = new TreeNode("Spot");
            //spotNode.ImageIndex = 1;
            //spotNode.SelectedImageIndex = 1;
            // Landscape
            //var landscapeBaseNode = new TreeNode("Landscape");
            //var landscapeNode = new TreeNode("Landscape");
            //landscapeNode.ImageIndex = 9;
            //landscapeNode.SelectedImageIndex = 9;
            // Trigger
            var triggerBaseNode = new TreeNode("Trigger");
            var triggerNode = new TreeNode("Trigger");
            triggerNode.ImageIndex = 10;
            triggerNode.SelectedImageIndex = 10;

            skyNode.Nodes.Add(skyboxNode);
            skyNode.Nodes.Add(skysphereNode);
            waterBaseNode.Nodes.Add(waterNode);
            lightsNode.Nodes.Add(directionalNode);
            lightsNode.Nodes.Add(pointNode);
            //lightsNode.Nodes.Add(spotNode);
            //landscapeBaseNode.Nodes.Add(landscapeNode);
            triggerBaseNode.Nodes.Add(triggerNode);
            node.Nodes.Add(skyNode);
            node.Nodes.Add(waterBaseNode);
            node.Nodes.Add(lightsNode);
            //node.Nodes.Add(landscapeBaseNode);
            node.Nodes.Add(triggerBaseNode);
            tvObjects.Nodes.Add(node);

            // Load from directory.
            tvObjects.BeginUpdate();
            SetNode(tvObjects, projectPath, projectPath.Remove(0, projectPath.LastIndexOf("\\") + 1));
            tvObjects.EndUpdate();
        }

        private void InitialiseSceneTreeView()
        {
            tvSceneObjects.Nodes.Clear();
            // Add lights to common objects.
            sceneObjectsNode = new TreeNode("Objects");
            sceneSkyNode = new TreeNode("Sky");
            sceneLightsNode = new TreeNode("Lights");
            sceneSoundsNode = new TreeNode("Sounds");
            sceneWaterNode = new TreeNode("Water");
            //sceneLandscapeNode = new TreeNode("Landscape");
            sceneTriggersNode = new TreeNode("Triggers");
            sceneParticlesNode = new TreeNode("Particles");

            tvSceneObjects.Nodes.Add(sceneSkyNode);
            tvSceneObjects.Nodes.Add(sceneObjectsNode);
            tvSceneObjects.Nodes.Add(sceneLightsNode);
            tvSceneObjects.Nodes.Add(sceneSoundsNode);
            tvSceneObjects.Nodes.Add(sceneWaterNode);
            //tvSceneObjects.Nodes.Add(sceneLandscapeNode);
            tvSceneObjects.Nodes.Add(sceneTriggersNode);
            tvSceneObjects.Nodes.Add(sceneParticlesNode);
        }

        private void AddToSceneTreeView(TreeNode node, ObjectBase obj)
        {
            var tempNode = new TreeNode(obj.Name);
            tempNode.Name = obj.UniqueId;
            int imageIndex = 1;

            if (obj.GetType().Equals(typeof(DirectionalLight)))
                imageIndex = 2;
            else if (obj.GetType().Equals(typeof(PointLight)))
                imageIndex = 3;
            else if (obj.GetType().Equals(typeof(Sound)))
                imageIndex = 4;
            else if (obj.GetType().Equals(typeof(Engine.Mesh)))
                imageIndex = 5;
            else if (obj.GetType().Equals(typeof(SkySphere)))
                imageIndex = 6;
            else if (obj.GetType().Equals(typeof(SkyBox)))
                imageIndex = 7;
            else if (obj.GetType().Equals(typeof(Water)))
                imageIndex = 8;
            else if (obj.GetType().Equals(typeof(Landscape)))
                imageIndex = 9;
            else if (obj.GetType().Equals(typeof(Trigger)))
                imageIndex = 10;
            else if (obj.GetType().Equals(typeof(Particle)))
                imageIndex = 11;

            tempNode.ImageIndex = imageIndex;
            tempNode.SelectedImageIndex = imageIndex;
            node.Nodes.Add(tempNode);
        }

        private void SetNode(TreeView treeName, string root, string nodeText)
        {
            var dirInfo = new DirectoryInfo(root);
            var node = new TreeNode();
            node.Text = Helpers.FirstLetterToUpper(nodeText);
            GetFolders(dirInfo, node);
            treeName.Nodes.Add(node);
            treeName.Nodes[1].Expand();
        }

        private void GetFolders(DirectoryInfo d, TreeNode node)
        {
            try
            {
                DirectoryInfo[] dInfo = d.GetDirectories();

                if (dInfo.Length > 0)
                {
                    var treeNode = new TreeNode();

                    foreach (DirectoryInfo driSub in dInfo)
                    {
                        if (driSub.Attributes == (FileAttributes.Hidden | FileAttributes.Directory))
                        {
                            continue;
                        }
                        else
                        {
                            treeNode = node.Nodes.Add(string.Empty, Helpers.FirstLetterToUpper(driSub.Name), 0);
                            GetFiles(driSub, treeNode);
                            GetFolders(driSub, treeNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetFiles(DirectoryInfo d, TreeNode node)
        {
            string lookfor = "*.tvm; *.tva; *.x; *.wav; *.mp3; *.ogg; *.tvp;";

            foreach (string extension in Settings.ScriptFileExtension.Split(';'))
            {
                lookfor += string.Format(" *.{0};", extension);
            }

            string[] extensions = lookfor.Split(new char[] { ';' });
            var myfileinfos = new ArrayList();
            foreach (string ext in extensions)
                myfileinfos.AddRange(d.GetFiles(ext.Trim()));

            var subfileInfo = myfileinfos.ToArray(typeof(FileInfo)) as FileInfo[];

            if (subfileInfo != null)
            {
                if (subfileInfo.Length > 0)
                {
                    for (int j = 0; j < subfileInfo.Length; j++)
                    {
                        var n = new TreeNode();
                        int imageIndex = 1;

                        if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.DirectionalLight)
                            imageIndex = 2;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.PointLight)
                            imageIndex = 3;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.Sound)
                            imageIndex = 4;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.StaticMesh ||
                            Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.AnimatedMesh)
                            imageIndex = 5;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.SkySphere)
                            imageIndex = 6;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.SkyBox)
                            imageIndex = 7;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.Water)
                            imageIndex = 8;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.Landscape)
                            imageIndex = 9;
                        else if (Helpers.GetObjectType(subfileInfo[j].FullName) == Helpers.ObjectType.Particle)
                            imageIndex = 11;

                        n.ImageIndex = imageIndex;
                        n.SelectedImageIndex = imageIndex;
                        n.Text = subfileInfo[j].Name;
                        n.Tag = string.Empty;
                        node.Nodes.Add(n);
                    }
                }
            }
        }

        private void MainLoop()
        {
            while (doLoop)
            {
                Application.DoEvents();

                if (mouseOver)
                {
                    UpdateInput();
                }
                else
                {
                    Cursor = Cursors.Default;
                }

                core.PreUpdate();
                if (core.PreviewingScene)
                {
                    core.Debug("Preview mode. Press escape to exit.\r\nPress F6 to reset.", 1);
                }
                else
                {
                    if (core.AllObjects.Count > 0)
                    {
                        core.Debug(string.Format("Total objects: {0}", core.AllObjects.Count.ToString()), 1);
                    }

                    if (selectedObject != null)
                    {
                        core.Debug(string.Format("Selected: {0}", selectedObject.Name), 2);
                    }
                }
                core.Update();
                DrawObjectLines();

                core.PostUpdate();
            }
        }

        private void DeselectAllObjects()
        {
            propertyGrid.SelectedObject = null;
            selectedObject = null;
            foreach (var ob in core.AllObjects)
            {
                ob.Deselect();
            }
        }

        private void UpdateMenu()
        {
            if (selectedObject != null)
            {
                btnRotateX.Enabled = true;
                btnRotateY.Enabled = true;
                btnRotateZ.Enabled = true;
                btnPutObjectOnGround.Enabled = true;
            }
            else
            {
                btnRotateX.Enabled = false;
                btnRotateY.Enabled = false;
                btnRotateZ.Enabled = false;
                btnPutObjectOnGround.Enabled = false;
            }
        }

        private TV_3DVECTOR TrimVector(TV_3DVECTOR vector)
        {
            if (Settings.SnapToGrid)
            {
                vector.x = (float)Math.Round(vector.x);
                vector.y = (float)Math.Round(vector.y);
                vector.z = (float)Math.Round(vector.z);
            }

            return vector;
        }

        private void SnapToGrid(TV_3DVECTOR newPosition)
        {
            if (Settings.SnapToGrid)
            {
                if (core.MathLibrary.GetDistanceVec3D(initialPosition, newPosition) >= Settings.SnapValue)
                {
                    if (Math.Abs(initialPosition.x - newPosition.x) != Settings.SnapValue)
                    {
                        newPosition.x = initialPosition.x;
                    }

                    if (Math.Abs(initialPosition.y - newPosition.y) != Settings.SnapValue)
                    {
                        newPosition.y = initialPosition.y;
                    }

                    if (Math.Abs(initialPosition.z - newPosition.z) != Settings.SnapValue)
                    {
                        newPosition.z = initialPosition.z;
                    }

                    initialPosition = newPosition;
                }
                else
                {
                    newPosition = initialPosition;
                }
            }
        }

        private void MoveHorizontal(int mouseAbsoluteX, int mouseAbsoluteY)
        {
            if (selectedObject != null)
            {
                TVMesh mesh = selectedObject.GetMesh();
                var vecNearRay = core.Globals.Vector3(0, 0, 0);
                var vecFarRay = core.Globals.Vector3(0, 0, 0);
                core.MathLibrary.GetMousePickVectors(mouseAbsoluteX, mouseAbsoluteY, ref vecNearRay, ref vecFarRay);
                TV_3DVECTOR newPosition = core.MathLibrary.IntersectionXZPlane(mesh.GetPosition().y, vecNearRay,
                                                                               vecFarRay);

                if (mousePosition3D.x == 0f && mousePosition3D.y == 0f && mousePosition3D.z == 0f)
                {
                    initialPosition = TrimVector(mesh.GetPosition());
                    core.MathLibrary.TVVec3Subtract(ref mousePosition3D, mesh.GetPosition(), newPosition);
                }
                newPosition = TrimVector(core.MathLibrary.VAdd(newPosition, mousePosition3D));

                SnapToGrid(newPosition);

                selectedObject.Position = new VECTOR3D(newPosition.x, newPosition.y, newPosition.z);
                selectedObject.Update();
                propertyGrid.SelectedObject = selectedObject;
            }
        }

        private void MoveVertical(int mouseAbsoluteX, int mouseAbsoluteY)
        {
            if (selectedObject != null)
            {
                TVMesh mesh = selectedObject.GetMesh();
                float pozX = 0f;
                float pozY = 0f;
                int mouseY = 0;
                var newPosition = core.Globals.Vector3(0, 0, 0);

                //TV_PLANE plane = new TV_PLANE();
                //core.Camera.GetFrustumPlanes(ref plane);

                core.MathLibrary.Project3DPointTo2D(mesh.GetPosition(), ref pozX, ref pozY, true);
                mouseY = (int)Math.Round(pozY);
                newPosition = mesh.GetPosition();
                newPosition.y = newPosition.y + (mouseY - mouseAbsoluteY) * 0.007f;

                if (mousePosition3D.x == 0f && mousePosition3D.y == 0f && mousePosition3D.z == 0f)
                {
                    initialPosition = TrimVector(mesh.GetPosition());
                    core.MathLibrary.TVVec3Subtract(ref mousePosition3D, mesh.GetPosition(), newPosition);
                }
                newPosition = TrimVector(core.MathLibrary.VAdd(newPosition, mousePosition3D));

                SnapToGrid(newPosition);

                selectedObject.Position = new VECTOR3D(newPosition.x, newPosition.y, newPosition.z);
                selectedObject.Update();
                propertyGrid.SelectedObject = selectedObject;
            }
        }

        private void DrawObjectLines()
        {
            if (selectedObject != null && selectedObject.GetMesh() != null)
            {
                float lengthX = 0f;
                float lengthY = 0f;
                float lengthZ = 0f;
                float length = 0f;
                var min = core.Globals.Vector3(0, 0, 0);
                var max = core.Globals.Vector3(0, 0, 0);
                var poz = core.Globals.Vector3(0, 0, 0);

                selectedObject.GetMesh().GetBoundingBox(ref min, ref max);
                core.MathLibrary.TVVec3Lerp(ref poz, min, max, 0.5f);
                lengthX = (max.x - min.x) * 2f;
                lengthY = (max.y - min.y) * 2f;
                lengthZ = (max.z - min.z) * 2f;

                length = lengthX;
                if (length < lengthY)
                    length = lengthY;
                if (length < lengthZ)
                    length = lengthZ;

                core.Screen2DImmediate.Draw_Line3D(poz.x - length, poz.y, poz.z, poz.x + length, poz.y, poz.z,
                                                   core.Globals.Colorkey(CONST_TV_COLORKEY.TV_COLORKEY_RED));
                core.Screen2DImmediate.Draw_Line3D(poz.x, poz.y - length, poz.z, poz.x, poz.y + length, poz.z,
                                                   core.Globals.Colorkey(CONST_TV_COLORKEY.TV_COLORKEY_GREEN));
                core.Screen2DImmediate.Draw_Line3D(poz.x, poz.y, poz.z - length, poz.x, poz.y, poz.z + length,
                                                   core.Globals.Colorkey(CONST_TV_COLORKEY.TV_COLORKEY_BLUE));
            }
        }

        private void ChangeMouseCursor()
        {
            switch (selectedTool)
            {
                case Tool.None:
                    Cursor = Cursors.Default;
                    break;
                case Tool.MoveHorizontal:
                    Cursor = Cursors.SizeWE;
                    break;
                case Tool.MoveVertical:
                    Cursor = Cursors.SizeNS;
                    break;
            }
        }

        private ObjectBase CreateObjectCopy(ObjectBase selectedObject)
        {
            Cursor tmpCursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;

            if (selectedObject.GetType().Equals(typeof(Engine.Mesh)))
            {
                var mesh = new Engine.Mesh(core, Settings, selectedObject.FileName);
                mesh.IsAnimated = (selectedObject as Engine.Mesh).IsAnimated;
                mesh.CustomTexture = (selectedObject as Engine.Mesh).CustomTexture;
                mesh.Visible = selectedObject.Visible;
                mesh.Position = selectedObject.Position;
                mesh.Rotation = selectedObject.Rotation;
                mesh.Scale = selectedObject.Scale;
                mesh.Mass = (selectedObject as Engine.Mesh).Mass;
                mesh.Bounding = (selectedObject as Engine.Mesh).Bounding;
                mesh.KineticFriction = (selectedObject as Engine.Mesh).KineticFriction;
                mesh.StaticFriction = (selectedObject as Engine.Mesh).StaticFriction;
                mesh.Bounciness = (selectedObject as Engine.Mesh).Bounciness;
                mesh.Softness = (selectedObject as Engine.Mesh).Softness;
                mesh.Material = (selectedObject as Engine.Mesh).Material;
                mesh.EnableLightning = (selectedObject as Engine.Mesh).EnableLightning;
                mesh.SetCustomCollection((selectedObject as Engine.Mesh).CustomParams);
                mesh.TextureScale = (selectedObject as Engine.Mesh).TextureScale;
                mesh.ScriptEnabled = (selectedObject as Engine.Mesh).ScriptEnabled;
                mesh.Script = (selectedObject as Engine.Mesh).Script.Replace((selectedObject as Engine.Mesh).Name, mesh.Name);
                mesh.Update();
                AddToSceneTreeView(sceneObjectsNode, mesh);
                Cursor = tmpCursor;
                return mesh;
            }

            if (selectedObject.GetType().Equals(typeof(PointLight)))
            {
                VECTOR3D pos = selectedObject.Position;
                var pointLight = (PointLight)selectedObject;
                var light = new PointLight(core, new TV_3DVECTOR(pos.X, pos.Y, pos.Z));
                light.Color = pointLight.Color;
                light.Radius = pointLight.Radius;
                light.Visible = pointLight.Visible;
                light.SetCustomCollection(pointLight.CustomParams);
                light.Update();
                AddToSceneTreeView(sceneLightsNode, light);
                Cursor = tmpCursor;
                return light;
            }

            if (selectedObject.GetType().Equals(typeof(Sound)))
            {
                var sound = new Sound(core, selectedObject.FileName);
                sound.Position = selectedObject.Position;
                sound.Stopped = (selectedObject as Sound).Stopped;
                sound.Volume = (selectedObject as Sound).Volume;
                sound.Loop = (selectedObject as Sound).Loop;
                sound.Is3D = (selectedObject as Sound).Is3D;
                sound.SetCustomCollection((selectedObject as Sound).CustomParams);
                sound.Update();
                AddToSceneTreeView(sceneSoundsNode, sound);
                Cursor = tmpCursor;
                return sound;
            }

            if (selectedObject.GetType().Equals(typeof(Engine.Trigger)))
            {
                var trigger = new Engine.Trigger(core);
                trigger.Position = selectedObject.Position;
                trigger.Rotation = selectedObject.Rotation;
                trigger.Scale = selectedObject.Scale;
                trigger.Color = (selectedObject as Trigger).Color;
                trigger.SetCustomCollection((selectedObject as Engine.Trigger).CustomParams);
                trigger.Update();
                AddToSceneTreeView(sceneTriggersNode, trigger);
                Cursor = tmpCursor;
                return trigger;
            }

            if (selectedObject.GetType().Equals(typeof(Engine.Particle)))
            {
                var particle = new Engine.Particle(core, selectedObject.FileName);
                particle.Position = selectedObject.Position;
                particle.Rotation = selectedObject.Rotation;
                particle.Scale = selectedObject.Scale;
                particle.Visible = selectedObject.Visible;
                particle.SetCustomCollection((selectedObject as Engine.Particle).CustomParams);
                particle.Update();
                AddToSceneTreeView(sceneParticlesNode, particle);
                Cursor = tmpCursor;
                return particle;
            }

            return null;
        }

        private void UpdateInput()
        {
            try
            {
                int mouseRelativeX = 0;
                int mouseRelativeY = 0;
                int mouseAbsoluteX = 0;
                int mouseAbsoluteY = 0;
                int mouseScroll = 0;
                bool mouseB1 = false;
                bool mouseB2 = false;
                bool mouseB3 = false;
                bool mouseB4 = false;

                core.Input.GetMouseState(ref mouseRelativeX, ref mouseRelativeY, ref mouseB1, ref mouseB2, ref mouseB3,
                                         ref mouseB4, ref mouseScroll);
                core.Input.GetAbsMouseState(ref mouseAbsoluteX, ref mouseAbsoluteY, ref mouseB1, ref mouseB2,
                                            ref mouseB3);
                core.CollisionResult = core.Scene.MousePick(mouseAbsoluteX, mouseAbsoluteY,
                                                            (int)CONST_TV_OBJECT_TYPE.TV_OBJECT_MESH);

                FPSCamera(0, 0, mouseScroll, false);
                if (mouseB1 && !core.PreviewingScene)
                {
                    if (selectedTool == Tool.None && core.CollisionResult.IsCollision())
                    {
                        DeselectAllObjects();

                        TVMesh colMesh = core.CollisionResult.GetCollisionMesh();
                        selectedObject = core.GetObjectByUniqueId(colMesh.GetMeshName());

                        selectedObject.Select();
                        propertyGrid.SelectedObject = selectedObject;
                    }

                    if (selectedObject != null)
                    {
                        if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_LEFTCONTROL) && !creatingObjectCopy)
                        {
                            creatingObjectCopy = true;
                            selectedObject = CreateObjectCopy(selectedObject);
                        }

                        switch (selectedTool)
                        {
                            case Tool.MoveHorizontal:
                                MoveHorizontal(mouseAbsoluteX, mouseAbsoluteY);
                                break;
                            case Tool.MoveVertical:
                                MoveVertical(mouseAbsoluteX, mouseAbsoluteY);
                                break;
                        }
                    }
                }
                else if (core.PreviewingScene)
                {
                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_ESCAPE))
                    {
                        core.PreviewingScene = false;
                        PrepareObjectsForPreview(false);
                        Thread.Sleep(200);
                    }
                    else if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_F6))
                    {
                        PreviewReset();
                    }
                    else
                    {
                        Cursor.Position = oldMousePosition;
                        pnlRenderer.Focus();
                        core.Engine.ShowWinCursor(false);
                        core.SoundFactory.StartAllSounds();
                        FPSCamera(mouseRelativeX, mouseRelativeY, mouseScroll, true);
                    }
                }
                else if (mouseB2)
                {
                    Cursor.Position = oldMousePosition;
                    pnlRenderer.Focus();
                    core.Engine.ShowWinCursor(false);
                    core.SoundFactory.StartAllSounds();
                    FPSCamera(mouseRelativeX, mouseRelativeY, mouseScroll, true);
                }
                else
                {
                    if (selectedTool != Tool.None)
                    {
                        DeselectAllObjects();

                        if (core.CollisionResult.IsCollision())
                        {
                            ChangeMouseCursor();
                            mousePosition3D = core.Globals.Vector3(0, 0, 0);
                            TVMesh colMesh = core.CollisionResult.GetCollisionMesh();
                            selectedObject = core.GetObjectByUniqueId(colMesh.GetMeshName());
                            selectedObject.Select();
                            propertyGrid.SelectedObject = selectedObject;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                        }
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_DELETE))
                    {
                        RemoveSelectedObject();
                    }
                    else if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_ESCAPE) ||
                             core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_SPACE))
                    {
                        core.PreviewingScene = false;
                        selectedTool = Tool.None;
                        Cursor = Cursors.Default;
                        DeselectAllObjects();
                    }
                    else if (!core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_LEFTCONTROL))
                    {
                        creatingObjectCopy = false;
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_Q))
                    {
                        SelectHorizontalTool();
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_E))
                    {
                        SelectVerticalTool();
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_C))
                    {
                        PutObjectOnGround();
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_F6))
                    {
                        StartPreview();
                    }

                    if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_F5))
                    {
                        RunProgram();
                    }

                    oldMousePosition = Cursor.Position;
                    core.Engine.ShowWinCursor(true);
                    core.SoundFactory.StopAllSounds();
                    UpdateMenu();
                }

                // Update all objects if not scene preview.
                if (!core.PreviewingScene)
                    core.AllObjects.ForEach(o => o.Update());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareObjectsForPreview(bool previewBegin)
        {
            foreach (ObjectBase obj in core.AllObjects)
            {
                if (obj.GetType().Equals(typeof(DirectionalLight)) ||
                    obj.GetType().Equals(typeof(PointLight)) ||
                    obj.GetType().Equals(typeof(Sound)))
                {
                    obj.Renderable = !previewBegin;
                }
            }

            if (previewBegin)
            {
                allMeshes = new List<Engine.Mesh>();
                foreach (ObjectBase obj in core.AllObjects)
                {
                    if (obj.GetType().Equals(typeof(Engine.Mesh)))
                    {
                        ((Engine.Mesh)obj).UpdatePhysics();
                        allMeshes.Add(obj as Engine.Mesh);
                    }
                }
            }
            else
            {
                foreach (Engine.Mesh m in allMeshes)
                {
                    foreach (ObjectBase o in core.AllObjects)
                    {
                        if (o.GetType().Equals(typeof(Engine.Mesh)) && o.UniqueId.Equals(m.UniqueId))
                        {
                            o.Position = m.Position;
                            o.Rotation = m.Rotation;
                            o.Scale = o.Scale;
                        }
                    }
                }
            }
        }

        private void RemoveSelectedObject()
        {
            if (selectedObject == null)
                return;

            var nodesToRemove = tvSceneObjects.Nodes.Find(selectedObject.UniqueId, true);
            foreach (TreeNode tn in nodesToRemove)
            {
                tvSceneObjects.Nodes.Remove(tn);
            }

            if (selectedObject.GetType().Equals(typeof(PointLight)))
            {
                core.LightEngine.DeleteLight(((PointLight)selectedObject).LightId);
            }
            else if (selectedObject.GetType().Equals(typeof(DirectionalLight)))
            {
                core.LightEngine.DeleteLight(((DirectionalLight)selectedObject).LightId);
            }
            else if (selectedObject.GetType().Equals(typeof(Sound)))
            {
                core.SoundFactory.RemoveSound(((Sound)selectedObject).GetSound());
            }
            else if (selectedObject.GetType().Equals(typeof(SkySphere)))
            {
                core.Atmosphere.SkySphere_SetTexture(-1);
                core.Atmosphere.SkySphere_Enable(false);
            }
            else if (selectedObject.GetType().Equals(typeof(SkyBox)))
            {
                core.Atmosphere.SkyBox_SetTexture(-1, -1, -1, -1, -1, -1);
                core.Atmosphere.SkyBox_Enable(false);
            }
            else if (selectedObject.GetType().Equals(typeof(Engine.Mesh)))
            {
                if (((Engine.Mesh)selectedObject).PhysicsId != -1)
                {
                    core.Physics.DestroyBody(((Engine.Mesh)selectedObject).PhysicsId, true);
                }
            }
            else if (selectedObject.GetType().Equals(typeof(Particle)))
            {
                (selectedObject as Particle).Destroy();
            }

            var meshToDestroy = selectedObject.GetMesh();
            core.AllObjects.RemoveAll(o => o.UniqueId == selectedObject.UniqueId);
            core.Scene.DestroyMesh(meshToDestroy);

            DeselectAllObjects();
            Thread.Sleep(200);
        }

        private void StartPreview()
        {
            if (mouseOver)
            {
                oldMousePosition = Cursor.Position;
            }
            else
            {
                oldMousePosition = new Point(Left + pnlRenderer.Width / 2, Top + pnlRenderer.Height / 2);
            }
            Cursor.Position = oldMousePosition;
            core.PreviewingScene = true;
            //DeselectAllObjects();
            PrepareObjectsForPreview(true);
        }

        private void PreviewReset()
        {
            foreach (Engine.Mesh m in allMeshes)
            {
                foreach (ObjectBase o in core.AllObjects)
                {
                    if (o.GetType().Equals(typeof(Engine.Mesh)) && o.UniqueId.Equals(m.UniqueId))
                    {
                        o.Position = m.Position;
                        o.Rotation = m.Rotation;
                        if (((Engine.Mesh)o).PhysicsId != -1)
                        {
                            core.Physics.SetBodyPosition(((Engine.Mesh)o).PhysicsId, o.Position.X, o.Position.Y, o.Position.Z);
                            core.Physics.SetBodyRotation(((Engine.Mesh)o).PhysicsId, o.Rotation.X, o.Rotation.Y, o.Rotation.Z);
                            core.Physics.SetBodyLinearVelocity(((Engine.Mesh)o).PhysicsId, new TV_3DVECTOR());
                            core.Physics.SetBodyAngularVelocity(((Engine.Mesh)o).PhysicsId, new TV_3DVECTOR());
                        }
                    }
                }
            }
        }

        private void RunProgram()
        {
            if (!string.IsNullOrEmpty(Settings.ProgramToRun))
            {
                if (File.Exists(Settings.ProgramToRun))
                {
                    if (string.IsNullOrEmpty(sceneFileName))
                    {
                        MessageBox.Show("Scene is not saved. You shoud save it first. Press OK to save it.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SaveScene(SaveMode.SaveAs);
                    }
                    else
                    {
                        SaveScene(SaveMode.Save);
                    }

                    if (!string.IsNullOrEmpty(sceneFileName))
                    {
                        core.Engine.SetSearchDirectory(Application.StartupPath);
                        this.WindowState = FormWindowState.Minimized;
                        string[] args = {
                                            sceneFileName, 
                                            core.Scene.GetCamera().GetPosition().x.ToString(),
                                            core.Scene.GetCamera().GetPosition().y.ToString(),
                                            core.Scene.GetCamera().GetPosition().z.ToString(),
                                            core.Scene.GetCamera().GetLookAt().x.ToString(),
                                            core.Scene.GetCamera().GetLookAt().y.ToString(),
                                            core.Scene.GetCamera().GetLookAt().z.ToString()
                                        };
                        var instance = new Process
                                           {
                                               StartInfo =
                                                   {
                                                       FileName = Settings.ProgramToRun,
                                                       Arguments = string.Join(" ", args),
                                                       WindowStyle = ProcessWindowStyle.Normal
                                                   }
                                           };

                        instance.Start();
                        instance.WaitForExit();
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
            }
            else
            {
                MessageBox.Show("Define a program to run.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateSelectedObject(ObjectBase obj)
        {
            obj.Update();
            propertyGrid.SelectedObject = obj;
        }

        private void pnlRenderer_SizeChanged(object sender, EventArgs e)
        {
            if (core != null)
                if (core.Engine != null)
                    core.Engine.ResizeDevice();
        }

        private void btnRefreshObjList_Click(object sender, EventArgs e)
        {
            LoadObjectInObjectsTreeView(Path.Combine(Application.StartupPath, Settings.FolderBase));
        }

        private void pnlRenderer_MouseLeave(object sender, EventArgs e)
        {
            mouseOver = false;
        }

        private void pnlRenderer_MouseEnter(object sender, EventArgs e)
        {
            mouseOver = true;
            pnlRenderer.Focus();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var mesh = ((PropertyGrid)(s)).SelectedObject as ObjectBase;
            if (mesh != null) mesh.Update();
            if (mesh.GetType().Equals(typeof(Engine.Mesh)))
            {
                ((Engine.Mesh)mesh).UpdatePhysics();
            }
            propertyGrid.Refresh();
        }

        private void btnToolDefault_Click(object sender, EventArgs e)
        {
            DeselectAllObjects();
            selectedTool = Tool.None;
        }

        private void btnToolVerticalMove_Click(object sender, EventArgs e)
        {
            SelectVerticalTool();
        }

        private void SelectVerticalTool()
        {
            DeselectAllObjects();
            selectedTool = Tool.MoveVertical;
        }

        private void btnToolHorizontalMove_Click(object sender, EventArgs e)
        {
            SelectHorizontalTool();
        }

        private void SelectHorizontalTool()
        {
            DeselectAllObjects();
            selectedTool = Tool.MoveHorizontal;
        }

        private void RunRegisteredExtension(string fullPath)
        {
            var extension = Path.GetExtension(fullPath).Replace(".", "");

            if (!extension.Equals(string.Empty))
            {
                foreach (string ext in Settings.ScriptFileExtension.Split(';'))
                {
                    if (ext.ToLower().Equals(extension))
                    {
                        using (System.Diagnostics.Process prc = new System.Diagnostics.Process())
                        {
                            prc.StartInfo.FileName = fullPath;
                            prc.Start();
                        }
                        break;
                    }
                }
            }
        }

        private void tvObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var type = Helpers.GetObjectType(e.Node.FullPath);
            var fullPath = Path.Combine(Application.StartupPath, e.Node.FullPath);
            var position = core.Camera.GetFrontPosition(10.0f);

            position.x = (float)Math.Round(position.x);
            position.y = (float)Math.Round(position.y);
            position.z = (float)Math.Round(position.z);

            bool skyBoxExists, skySphereExists = false;
            switch (type)
            {
                case Helpers.ObjectType.DirectionalLight:
                    var sunId = -1;
                    sunId = core.LightEngine.GetLightFromName(Helpers.SUN);
                    if (sunId == -1)
                    {
                        var directionalLight = new DirectionalLight(core, position);
                        directionalLight.Update();
                        selectedObject = directionalLight;
                        AddToSceneTreeView(sceneLightsNode, directionalLight);
                    }
                    else
                    {
                        MessageBox.Show("There should be only one directional light.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case Helpers.ObjectType.PointLight:
                    var pointLight = new PointLight(core, position);
                    pointLight.Update();
                    selectedObject = pointLight;
                    AddToSceneTreeView(sceneLightsNode, pointLight);
                    break;
                case Helpers.ObjectType.SpotLight:
                    MessageBox.Show("Not implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Helpers.ObjectType.StaticMesh:
                    var mesh = new Engine.Mesh(core, Settings, fullPath)
                                   {
                                       Position = new VECTOR3D(position.x, position.y, position.z)
                                   };
                    mesh.Update();
                    selectedObject = mesh;
                    AddToSceneTreeView(sceneObjectsNode, mesh);
                    break;
                case Helpers.ObjectType.AnimatedMesh:
                    var animatedMesh = new Engine.Mesh(core, Settings, fullPath)
                    {
                        Position = new VECTOR3D(position.x, position.y, position.z)
                    };
                    animatedMesh.Update();
                    selectedObject = animatedMesh;
                    AddToSceneTreeView(sceneObjectsNode, animatedMesh);
                    break;
                case Helpers.ObjectType.Sound:
                    var sound = new Sound(core, fullPath);
                    sound.Update();
                    selectedObject = sound;
                    AddToSceneTreeView(sceneSoundsNode, sound);
                    break;
                case Helpers.ObjectType.SkyBox:
                    skyBoxExists = core.AllObjects.FindLast(o => o.GetType() == typeof(SkyBox)) == null ? false : true;
                    if (skyBoxExists)
                        break;
                    skySphereExists = core.AllObjects.FindLast(o => o.GetType() == typeof(SkySphere)) == null ? false : true;
                    if (skySphereExists)
                    {
                        if (MessageBox.Show("This will remove existing sky sphere. Remove?", "Remove sky sphere?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            selectedObject = core.AllObjects.FindLast(o => o.GetType() == typeof(SkySphere));
                            RemoveSelectedObject();
                        }
                        else
                            break;
                    }
                    var skyBox = new SkyBox(core);
                    skyBox.Update();
                    AddToSceneTreeView(sceneSkyNode, skyBox);
                    selectedObject = skyBox;
                    break;
                case Helpers.ObjectType.SkySphere:
                    skySphereExists = core.AllObjects.FindLast(o => o.GetType() == typeof(SkySphere)) == null ? false : true;
                    if (skySphereExists)
                        break;
                    skyBoxExists = core.AllObjects.FindLast(o => o.GetType() == typeof(SkyBox)) == null ? false : true;
                    if (skyBoxExists)
                    {
                        if (MessageBox.Show("This will remove existing sky box. Remove?", "Remove sky box?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            selectedObject = core.AllObjects.FindLast(o => o.GetType() == typeof(SkyBox));
                            RemoveSelectedObject();
                        }
                        else
                            break;
                    }
                    var skySphere = new SkySphere(core);
                    skySphere.Update();
                    AddToSceneTreeView(sceneSkyNode, skySphere);
                    selectedObject = skySphere;
                    break;
                case Helpers.ObjectType.Water:
                    var water = new Water(core);
                    selectedObject = water;
                    AddToSceneTreeView(sceneWaterNode, water);
                    break;
                case Helpers.ObjectType.Landscape:
                    var landscape = new Landscape(core)
                    {
                        Position = new VECTOR3D(position.x, position.y, position.z)
                    };
                    landscape.Update();
                    selectedObject = landscape;
                    AddToSceneTreeView(sceneLandscapeNode, landscape);
                    break;
                case Helpers.ObjectType.Trigger:
                    var trigger = new Trigger(core)
                    {
                        Position = new VECTOR3D(position.x, position.y, position.z)
                    };
                    trigger.Update();
                    selectedObject = trigger;
                    AddToSceneTreeView(sceneTriggersNode, trigger);
                    break;
                case Helpers.ObjectType.Particle:
                    var particle = new Particle(core, fullPath);
                    particle.Update();
                    selectedObject = particle;
                    AddToSceneTreeView(sceneParticlesNode, particle);
                    break;
                case Helpers.ObjectType.NotDetected:
                    RunRegisteredExtension(fullPath);
                    break;
            }

            propertyGrid.SelectedObject = selectedObject;

            Cursor = Cursors.Default;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var setings = new frmSettings(Settings);
            setings.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sceneFileName.Equals(string.Empty))
            {
                SaveScene(SaveMode.SaveAs);
            }
            else
            {
                SaveScene(SaveMode.Save);
            }
        }

        private void ChangeProgramTitle(string fileName)
        {
            if (fileName.Equals(string.Empty))
            {
                Text = defaultProductName;
            }
            else
            {
                string file = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
                Text = string.Format("{0} - {1}", defaultProductName, file);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, Settings.FolderLevels.Replace("/", "\\"));
            openFileDialog.Filter = "Sandbox files (*.xml, *.json)|*.xml;*.json|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Cursor tmpCursor = Cursor.Current;
                Cursor = Cursors.WaitCursor;
                ClearScene();
                core.LoadScene(openFileDialog.FileName);
                sceneFileName = openFileDialog.FileName;
                Cursor = tmpCursor;

                foreach (ObjectBase obj in core.AllObjects)
                {
                    TreeNode tn = null;

                    if (obj.GetType().Equals(typeof(Engine.Mesh)))
                    {
                        tn = sceneObjectsNode;
                    }
                    else if (obj.GetType().Equals(typeof(DirectionalLight)) ||
                        obj.GetType().Equals(typeof(PointLight)))
                    {
                        tn = sceneLightsNode;
                    }
                    else if (obj.GetType().Equals(typeof(Sound)))
                    {
                        tn = sceneSoundsNode;
                    }
                    else if (obj.GetType().Equals(typeof(Water)))
                    {
                        tn = sceneWaterNode;
                    }
                    else if (obj.GetType().Equals(typeof(Landscape)))
                    {
                        tn = sceneLandscapeNode;
                    }
                    else if (obj.GetType().Equals(typeof(Trigger)))
                    {
                        tn = sceneTriggersNode;
                    }
                    else if (obj.GetType().Equals(typeof(Particle)))
                    {
                        tn = sceneParticlesNode;
                    }
                    else if (obj.GetType().Equals(typeof(SkySphere)) ||
                        obj.GetType().Equals(typeof(SkyBox)))
                    {
                        tn = sceneSkyNode;
                    }

                    if (tn != null)
                    {
                        AddToSceneTreeView(tn, obj);
                    }
                }
            }

            ChangeProgramTitle(sceneFileName);
        }

        private void ClearScene()
        {
            DeselectAllObjects();
            foreach (ObjectBase o in core.AllObjects)
            {
                if (o.GetType().Equals(typeof(Engine.Mesh)))
                {
                    if (((Engine.Mesh)o).PhysicsId != -1)
                    {
                        core.Physics.DestroyBody(((Engine.Mesh)o).PhysicsId, true);
                    }

                    o.GetMesh().Destroy();
                }
            }
            foreach (Particle obj in core.AllObjects.FindAll(o => o.GetType().Equals(
                                                                       typeof(Particle))))
            {
                obj.Destroy();
            }
            core.AllObjects = new List<ObjectBase>();
            core.Scene.DestroyAllMeshes();
            core.LightEngine.DeleteAllLights();
            core.SoundFactory.RemoveAllSounds();
            core.Atmosphere.SkyBox_SetTexture(-1, -1, -1, -1, -1, -1);
            core.Atmosphere.SkyBox_Enable(false);
            core.Atmosphere.SkySphere_SetTexture(-1);
            core.Atmosphere.SkySphere_Enable(false);
            core.MaterialFactory.DeleteAllMaterials();
            sceneFileName = string.Empty;
            core.ResetErrorMessages();
            core.IsSkySphere = false;
            core.IsSkyBox = false;
            tvSceneObjects.Nodes.Clear();
            InitialiseSceneTreeView();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to create a new scene?", "New",
                                                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ChangeProgramTitle(string.Empty);
                ClearScene();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveScene(SaveMode.SaveAs);
        }

        private void SaveScene(SaveMode saveMode)
        {
            Cursor tmpCursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;

            if (saveMode == SaveMode.SaveAs)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, Settings.FolderLevels.Replace("/", "\\"));
                saveFileDialog.Filter = "Sandbox files (*.xml, *.json)|*.xml;*.json|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    core.SaveScene(saveFileDialog.FileName);
                    sceneFileName = saveFileDialog.FileName;
                }
            }
            else
            {
                core.SaveScene(sceneFileName);
            }

            ChangeProgramTitle(sceneFileName);
            Cursor = tmpCursor;
        }

        #region Nested type: SaveMode

        private enum SaveMode
        {
            Save,
            SaveAs
        }

        #endregion

        #region Nested type: Tool

        private enum Tool
        {
            None,
            MoveHorizontal,
            MoveVertical
        }

        #endregion

        private void btnPreview_Click(object sender, EventArgs e)
        {
            StartPreview();
        }

        private void tvSceneObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var obj = core.GetObjectByUniqueId(e.Node.Name);

            if (obj == null)
                return;

            if (obj.GetType().Equals(typeof(SkySphere)) || obj.GetType().Equals(typeof(SkyBox)))
                return;

            if (SelectObject(obj))
            {
                float radius = 0f;
                TV_3DVECTOR newPos = core.Globals.Vector3(0, 0, 0);
                selectedObject.GetMesh().GetBoundingSphere(ref newPos, ref radius);
                newPos = selectedObject.GetMesh().GetPosition();
                core.Camera.SetPosition(newPos.x + radius * 1.8f, newPos.y + radius * 1.2f, newPos.z);
                core.Camera.LookAtMesh(selectedObject.GetMesh());
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedObject = core.GetObjectByUniqueId(tvSceneObjects.SelectedNode.Name);

            if (selectedObject != null)
            {
                RemoveSelectedObject();
            }
        }

        private void tvSceneObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var obj = core.GetObjectByUniqueId(e.Node.Name);
            SelectObject(obj);
        }

        private bool SelectObject(ObjectBase obj)
        {
            if (obj != null)
            {
                var o = core.GetObjectByUniqueId(obj.UniqueId);

                if (o != null)
                {
                    DeselectAllObjects();
                    selectedObject = o;
                    selectedObject.Select();
                    propertyGrid.SelectedObject = selectedObject;

                    return true;
                }

                return false;
            }

            return false;
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            Settings.SnapToGrid = !Settings.SnapToGrid;
            btnSnap.Checked = Settings.SnapToGrid;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new frmAbout();
            about.ShowDialog();
        }

        private void saveSelectedAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedObject == null)
            {
                MessageBox.Show("Please select an object first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor tmpCursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, Settings.FolderModels);
            saveFileDialog.Filter = "X files (*.x)|*.x|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = selectedObject.Name.Remove(selectedObject.Name.IndexOf('.') + 1,
                selectedObject.Name.Length - selectedObject.Name.IndexOf('.') - 1) + "x";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveX(selectedObject.GetMesh(), saveFileDialog.FileName);
            }

            Cursor = tmpCursor;
        }

        private ColorValue ToDx(TV_COLOR tvc)
        {
            // Convert a TV Color to a DX Color.
            return new ColorValue(tvc.r, tvc.g, tvc.b, tvc.a);
        }

        private void SaveX(TVMesh tvm, string fileName)
        {
            ExtendedMaterial[] exMaterials = new ExtendedMaterial[tvm.GetGroupCount()];
            Material dxMaterial;
            int idx = 0;

            for (int group = 0; group < tvm.GetGroupCount(); group++)
            {
                idx = tvm.GetMaterial(group);
                dxMaterial = new Material();
                dxMaterial.AmbientColor = ToDx(core.MaterialFactory.GetAmbient(idx));
                dxMaterial.DiffuseColor = ToDx(core.MaterialFactory.GetDiffuse(idx));
                dxMaterial.EmissiveColor = ToDx(core.MaterialFactory.GetEmissive(idx));
                dxMaterial.SpecularColor = ToDx(core.MaterialFactory.GetSpecular(idx));
                dxMaterial.SpecularSharpness = core.MaterialFactory.GetPower(idx);
                exMaterials[group].Material3D = dxMaterial;
                // Get the Texture Filename.
                idx = tvm.GetTexture(group);
                // Add em to the array.
                exMaterials[group].TextureFilename = core.TextureFactory.GetTextureInfo(idx).Name;
            }

            // Save the Mesh.
            Microsoft.DirectX.Direct3D.Mesh dxMesh = new Microsoft.DirectX.Direct3D.Mesh(core.InternalObjects.GetD3DMesh(tvm.GetIndex()));
            int[] adjacency = new int[dxMesh.NumberFaces * 3];
            dxMesh.GenerateAdjacency(0f, adjacency);
            dxMesh.Save(fileName, adjacency, exMaterials, XFileFormat.Text);
        }

        private void btnPutObjectOnGround_Click(object sender, EventArgs e)
        {
            PutObjectOnGround();
        }

        private void PutObjectOnGround()
        {
            if (selectedObject != null)
            {
                TV_3DVECTOR min = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR max = core.Globals.Vector3(0, 0, 0);
                selectedObject.GetMesh().GetBoundingBox(ref min, ref max);

                TV_3DVECTOR center = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR centerLow = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR start1 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR start2 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR start3 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR start4 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR start5 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR end1 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR end2 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR end3 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR end4 = core.Globals.Vector3(0, 0, 0);
                TV_3DVECTOR end5 = core.Globals.Vector3(0, 0, 0);

                core.MathLibrary.TVVec3Lerp(ref center, min, max, 0.5f);
                centerLow = center;
                centerLow.y = min.y;
                start1 = min;
                start2 = max; start2.y = min.y;
                start3.x = max.x; start3.y = min.y; start3.z = min.z;
                start4.x = min.x; start4.y = min.y; start4.z = max.z;
                start5 = center; start5.y = min.y;

                end1 = start1;
                end2 = start2;
                end3 = start3;
                end4 = start4;
                end5 = start5;
                end1.y -= 100000;
                end2.y -= 100000;
                end3.y -= 100000;
                end4.y -= 100000;
                end5.y -= 100000;

                TV_COLLISIONRESULT coll1 = new TV_COLLISIONRESULT();
                TV_COLLISIONRESULT coll2 = new TV_COLLISIONRESULT();
                TV_COLLISIONRESULT coll3 = new TV_COLLISIONRESULT();
                TV_COLLISIONRESULT coll4 = new TV_COLLISIONRESULT();
                TV_COLLISIONRESULT coll5 = new TV_COLLISIONRESULT();

                core.Scene.AdvancedCollision(start1, end1, ref coll1);
                core.Scene.AdvancedCollision(start2, end2, ref coll2);
                core.Scene.AdvancedCollision(start3, end3, ref coll3);
                core.Scene.AdvancedCollision(start4, end4, ref coll4);
                core.Scene.AdvancedCollision(start5, end5, ref coll5);

                if (coll1.bHasCollided || coll2.bHasCollided ||
                    coll3.bHasCollided || coll4.bHasCollided || coll5.bHasCollided)
                {
                    TV_3DVECTOR newPos = selectedObject.GetMesh().GetPosition();

                    List<float> array = new List<float>();

                    if (coll1.bHasCollided)
                        array.Add(coll1.vCollisionImpact.y);
                    if (coll2.bHasCollided)
                        array.Add(coll2.vCollisionImpact.y);
                    if (coll3.bHasCollided)
                        array.Add(coll3.vCollisionImpact.y);
                    if (coll4.bHasCollided)
                        array.Add(coll4.vCollisionImpact.y);
                    if (coll5.bHasCollided)
                        array.Add(coll5.vCollisionImpact.y);

                    array.Sort();
                    newPos.y = array[array.Count - 1] + (max.y - min.y) / 2;

                    selectedObject.Position = new VECTOR3D(newPos.x, newPos.y, newPos.z);
                    selectedObject.Update();

                    Thread.Sleep(200);
                }
            }
        }

        private void Helper(TV_3DVECTOR position)
        {
            Helper(position, CONST_TV_COLORKEY.TV_COLORKEY_MAGENTA);
        }

        private void Helper(TV_3DVECTOR position, CONST_TV_COLORKEY color)
        {
            TVMesh m = core.Scene.CreateMeshBuilder();
            m.CreateSphere(0.25f);
            m.SetColor(core.Globals.Colorkey(color));
            m.SetLightingMode(CONST_TV_LIGHTINGMODE.TV_LIGHTING_MANAGED);
            m.SetPosition(position.x, position.y, position.z);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunProgram();
        }

        private void pnlRenderer_DoubleClick(object sender, EventArgs e)
        {
            if (selectedObject != null)
            {
                var form = new frmScript();
                form.Script = selectedObject.Script;
                form.Width = (int)Math.Round(pnlRenderer.Width * 0.8, 0);
                form.Height = (int)Math.Round(pnlRenderer.Height * 0.8, 0);
                form.ShowDialog();

                selectedObject.Script = form.Script;
            }
        }

        //ISoundEngine tmpSoundEngine = new ISoundEngine();
        //ISound tmpSound;
        private void tvObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var type = Helpers.GetObjectType(e.Node.FullPath);
                var fullPath = Path.Combine(Application.StartupPath, e.Node.FullPath);

                switch (type)
                {
                    case Helpers.ObjectType.Sound:
                        Thread t = new Thread(new ThreadStart(() =>
                            {
                                var tmpSound = core.SoundFactory.Load(fullPath, false);
                                tmpSound.Loop = false;
                                core.SoundFactory.PlaySound(tmpSound);
                                DateTime oldDate = DateTime.Now;
                                while (core.SoundFactory.IsPlaying(tmpSound))
                                {
                                    System.Threading.Thread.Sleep(10);
                                    DateTime newDate = DateTime.Now;
                                    TimeSpan ts = newDate - oldDate;
                                    if (ts.Seconds > 3)
                                    {
                                        core.SoundFactory.StopSound(tmpSound);
                                    }
                                }
                                core.SoundFactory.RemoveSound(tmpSound);
                            }
                            ));
                        t.Start();
                        break;
                }
            }
        }

        private void btnRotateX_MouseDown(object sender, MouseEventArgs e)
        {
            RotateObject(1, e);
        }

        private void btnRotateY_MouseDown(object sender, MouseEventArgs e)
        {
            RotateObject(2, e);
        }

        private void btnRotateZ_MouseDown(object sender, MouseEventArgs e)
        {
            RotateObject(3, e);
        }

        // 1 - X, 2 - Y, 3 - Z
        private void RotateObject(int axis, MouseEventArgs e)
        {
            if (selectedObject != null)
            {
                VECTOR3D position = selectedObject.Position;
                TV_3DMATRIX mat = new TV_3DMATRIX();
                TV_3DMATRIX modyMat = selectedObject.GetMesh().GetMatrix();
                var rotation = Settings.RotationValue;
                if (e.Button == MouseButtons.Right)
                    rotation = -rotation;
                if (axis == 1)
                    core.MathLibrary.TVMatrixRotationX(ref mat, rotation);
                else if (axis == 2)
                    core.MathLibrary.TVMatrixRotationY(ref mat, rotation);
                else if (axis == 3)
                    core.MathLibrary.TVMatrixRotationZ(ref mat, rotation);
                else return;
                core.MathLibrary.TVMatrixMultiply(ref mat, modyMat, mat);
                selectedObject.GetMesh().SetMatrix(mat);
                selectedObject.Position = position;
                selectedObject.Rotation = new VECTOR3D(selectedObject.GetMesh().GetRotation());
                UpdateSelectedObject(selectedObject);
            }
        }
    }
}