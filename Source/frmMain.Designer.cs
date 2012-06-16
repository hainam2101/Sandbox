namespace Sandbox
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Visual Studio 2003");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Luna Blue");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Luna Silver");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Luna Olive");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Classic");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Office 2003", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Luna Blue");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Luna Silver");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Luna Olive");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Classic");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Visual Studio 2005", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Blue");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Black");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Silver");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Office 2007", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14});
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnToolDefault = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolHorizontalMove = new System.Windows.Forms.ToolStripButton();
            this.btnToolVerticalMove = new System.Windows.Forms.ToolStripButton();
            this.btnPutObjectOnGround = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSnap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRotateX = new System.Windows.Forms.ToolStripButton();
            this.btnRotateY = new System.Windows.Forms.ToolStripButton();
            this.btnRotateZ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.sandDockManager = new TD.SandDock.SandDockManager();
            this.dockContainer2 = new TD.SandDock.DockContainer();
            this.dwObjects = new TD.SandDock.DockableWindow();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.objToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnRefreshObjList = new System.Windows.Forms.ToolStripButton();
            this.dwScene = new TD.SandDock.DockableWindow();
            this.tvSceneObjects = new System.Windows.Forms.TreeView();
            this.cmsSceneObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dwProperties = new TD.SandDock.DockableWindow();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlRenderer = new System.Windows.Forms.Panel();
            this.dockContainer = new TD.SandDock.DockContainer();
            this.dwAppearance = new TD.SandDock.DockableWindow();
            this.tvwRenderer = new System.Windows.Forms.TreeView();
            this.dwNewWindows = new TD.SandDock.DockableWindow();
            this.dwBehavior = new TD.SandDock.DockableWindow();
            this.chkIntegralClose = new System.Windows.Forms.CheckBox();
            this.chkAllowOptions = new System.Windows.Forms.CheckBox();
            this.chkShowActiveFilesList = new System.Windows.Forms.CheckBox();
            this.chkAllowDockContainerResize = new System.Windows.Forms.CheckBox();
            this.chkAllowClose = new System.Windows.Forms.CheckBox();
            this.chkAllowPin = new System.Windows.Forms.CheckBox();
            this.dwClassView = new TD.SandDock.DockableWindow();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.dwObjects.SuspendLayout();
            this.objToolStrip.SuspendLayout();
            this.dwScene.SuspendLayout();
            this.cmsSceneObjects.SuspendLayout();
            this.dwProperties.SuspendLayout();
            this.dockContainer.SuspendLayout();
            this.dwAppearance.SuspendLayout();
            this.dwBehavior.SuspendLayout();
            this.dwClassView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(675, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator7,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveSelectedAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.optionsToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(165, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveSelectedAsToolStripMenuItem
            // 
            this.saveSelectedAsToolStripMenuItem.Name = "saveSelectedAsToolStripMenuItem";
            this.saveSelectedAsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveSelectedAsToolStripMenuItem.Text = "Save selected as X";
            this.saveSelectedAsToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(165, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolDefault,
            this.toolStripSeparator1,
            this.btnToolHorizontalMove,
            this.btnToolVerticalMove,
            this.btnPutObjectOnGround,
            this.toolStripSeparator2,
            this.btnSnap,
            this.toolStripSeparator6,
            this.btnRotateX,
            this.btnRotateY,
            this.btnRotateZ,
            this.toolStripSeparator5,
            this.btnRun,
            this.btnPreview});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(675, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // btnToolDefault
            // 
            this.btnToolDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolDefault.Image = ((System.Drawing.Image)(resources.GetObject("btnToolDefault.Image")));
            this.btnToolDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolDefault.Name = "btnToolDefault";
            this.btnToolDefault.Size = new System.Drawing.Size(23, 22);
            this.btnToolDefault.Text = "Switch to object select mode.";
            this.btnToolDefault.Click += new System.EventHandler(this.btnToolDefault_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolHorizontalMove
            // 
            this.btnToolHorizontalMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolHorizontalMove.Image = global::Sandbox.Properties.Resources.cursorHorizontal;
            this.btnToolHorizontalMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolHorizontalMove.Name = "btnToolHorizontalMove";
            this.btnToolHorizontalMove.Size = new System.Drawing.Size(23, 22);
            this.btnToolHorizontalMove.Text = "Move object in horizontal direction (Q).";
            this.btnToolHorizontalMove.Click += new System.EventHandler(this.btnToolHorizontalMove_Click);
            // 
            // btnToolVerticalMove
            // 
            this.btnToolVerticalMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolVerticalMove.Image = global::Sandbox.Properties.Resources.cursorVertical;
            this.btnToolVerticalMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolVerticalMove.Name = "btnToolVerticalMove";
            this.btnToolVerticalMove.Size = new System.Drawing.Size(23, 22);
            this.btnToolVerticalMove.Text = "Move object in vertical direction (E).";
            this.btnToolVerticalMove.Click += new System.EventHandler(this.btnToolVerticalMove_Click);
            // 
            // btnPutObjectOnGround
            // 
            this.btnPutObjectOnGround.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPutObjectOnGround.Enabled = false;
            this.btnPutObjectOnGround.Image = global::Sandbox.Properties.Resources.putObjOnObj;
            this.btnPutObjectOnGround.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPutObjectOnGround.Name = "btnPutObjectOnGround";
            this.btnPutObjectOnGround.Size = new System.Drawing.Size(23, 22);
            this.btnPutObjectOnGround.Text = "Put selected object on a ground (C).";
            this.btnPutObjectOnGround.Click += new System.EventHandler(this.btnPutObjectOnGround_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSnap
            // 
            this.btnSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSnap.Image = global::Sandbox.Properties.Resources.snap;
            this.btnSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(23, 22);
            this.btnSnap.Text = "Turn snap on or off.";
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRotateX
            // 
            this.btnRotateX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateX.Enabled = false;
            this.btnRotateX.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateX.Image")));
            this.btnRotateX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateX.Name = "btnRotateX";
            this.btnRotateX.Size = new System.Drawing.Size(23, 22);
            this.btnRotateX.Text = "Rotate selected object around X axis.";
            this.btnRotateX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRotateX_MouseDown);
            // 
            // btnRotateY
            // 
            this.btnRotateY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateY.Enabled = false;
            this.btnRotateY.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateY.Image")));
            this.btnRotateY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateY.Name = "btnRotateY";
            this.btnRotateY.Size = new System.Drawing.Size(23, 22);
            this.btnRotateY.Text = "Rotate selected object around Y axis.";
            this.btnRotateY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRotateY_MouseDown);
            // 
            // btnRotateZ
            // 
            this.btnRotateZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateZ.Enabled = false;
            this.btnRotateZ.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateZ.Image")));
            this.btnRotateZ.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateZ.Name = "btnRotateZ";
            this.btnRotateZ.Size = new System.Drawing.Size(23, 22);
            this.btnRotateZ.Text = "Rotate selected object around Z axis.";
            this.btnRotateZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRotateZ_MouseDown);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = global::Sandbox.Properties.Resources.run;
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(23, 22);
            this.btnRun.Text = "Run with external program (F5).";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreview.Image = global::Sandbox.Properties.Resources.preview;
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(23, 22);
            this.btnPreview.Text = "Start preview (F6).";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // sandDockManager
            // 
            this.sandDockManager.AutoSaveLayout = true;
            this.sandDockManager.DockSystemContainer = this;
            this.sandDockManager.OwnerForm = this;
            this.sandDockManager.SelectTabsOnDrag = true;
            // 
            // dockContainer2
            // 
            this.dockContainer2.ContentSize = 250;
            this.dockContainer2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockContainer2.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[0]);
            this.dockContainer2.Location = new System.Drawing.Point(0, 0);
            this.dockContainer2.Manager = null;
            this.dockContainer2.Name = "dockContainer2";
            this.dockContainer2.Size = new System.Drawing.Size(0, 0);
            this.dockContainer2.TabIndex = 0;
            // 
            // dwObjects
            // 
            this.dwObjects.AllowClose = false;
            this.dwObjects.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwObjects.Controls.Add(this.tvObjects);
            this.dwObjects.Controls.Add(this.objToolStrip);
            this.dwObjects.Guid = new System.Guid("8c3e20d6-712e-4964-9d13-e87ec09b03ac");
            this.dwObjects.Location = new System.Drawing.Point(4, 18);
            this.dwObjects.Name = "dwObjects";
            this.dwObjects.Size = new System.Drawing.Size(263, 79);
            this.dwObjects.TabIndex = 1;
            this.dwObjects.Text = "Objects";
            // 
            // tvObjects
            // 
            this.tvObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.ImageIndex = 0;
            this.tvObjects.ImageList = this.imageList;
            this.tvObjects.Location = new System.Drawing.Point(1, 26);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.SelectedImageIndex = 0;
            this.tvObjects.Size = new System.Drawing.Size(261, 52);
            this.tvObjects.TabIndex = 0;
            this.tvObjects.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvObjects_NodeMouseDoubleClick);
            this.tvObjects.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvObjects_NodeMouseClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder_closed_16x16.gif");
            this.imageList.Images.SetKeyName(1, "file_16x16.gif");
            this.imageList.Images.SetKeyName(2, "sun.png");
            this.imageList.Images.SetKeyName(3, "point.png");
            this.imageList.Images.SetKeyName(4, "snd.png");
            this.imageList.Images.SetKeyName(5, "SandBox.ico");
            this.imageList.Images.SetKeyName(6, "sphere.png");
            this.imageList.Images.SetKeyName(7, "skybox.png");
            this.imageList.Images.SetKeyName(8, "wtr.png");
            this.imageList.Images.SetKeyName(9, "landscape.png");
            this.imageList.Images.SetKeyName(10, "trigger.png");
            this.imageList.Images.SetKeyName(11, "particle.png");
            // 
            // objToolStrip
            // 
            this.objToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshObjList});
            this.objToolStrip.Location = new System.Drawing.Point(1, 1);
            this.objToolStrip.Name = "objToolStrip";
            this.objToolStrip.Size = new System.Drawing.Size(261, 25);
            this.objToolStrip.TabIndex = 1;
            this.objToolStrip.Text = "toolStrip";
            // 
            // btnRefreshObjList
            // 
            this.btnRefreshObjList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefreshObjList.Image = global::Sandbox.Properties.Resources.refresh;
            this.btnRefreshObjList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshObjList.Name = "btnRefreshObjList";
            this.btnRefreshObjList.Size = new System.Drawing.Size(23, 22);
            this.btnRefreshObjList.Text = "Refresh object list.";
            this.btnRefreshObjList.Click += new System.EventHandler(this.btnRefreshObjList_Click);
            // 
            // dwScene
            // 
            this.dwScene.AllowClose = false;
            this.dwScene.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwScene.Controls.Add(this.tvSceneObjects);
            this.dwScene.Guid = new System.Guid("1ec510b6-723b-48a5-9d54-9e2704c8b1ae");
            this.dwScene.Location = new System.Drawing.Point(4, 143);
            this.dwScene.Name = "dwScene";
            this.dwScene.Size = new System.Drawing.Size(263, 79);
            this.dwScene.TabIndex = 0;
            this.dwScene.Text = "Scene";
            // 
            // tvSceneObjects
            // 
            this.tvSceneObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvSceneObjects.ContextMenuStrip = this.cmsSceneObjects;
            this.tvSceneObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSceneObjects.ImageIndex = 0;
            this.tvSceneObjects.ImageList = this.imageList;
            this.tvSceneObjects.Location = new System.Drawing.Point(1, 1);
            this.tvSceneObjects.Name = "tvSceneObjects";
            this.tvSceneObjects.SelectedImageIndex = 0;
            this.tvSceneObjects.Size = new System.Drawing.Size(261, 77);
            this.tvSceneObjects.TabIndex = 1;
            this.tvSceneObjects.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSceneObjects_NodeMouseDoubleClick);
            this.tvSceneObjects.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSceneObjects_NodeMouseClick);
            // 
            // cmsSceneObjects
            // 
            this.cmsSceneObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.cmsSceneObjects.Name = "cmsSceneObjects";
            this.cmsSceneObjects.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripMenuItem.Image")));
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // dwProperties
            // 
            this.dwProperties.AllowClose = false;
            this.dwProperties.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwProperties.Controls.Add(this.propertyGrid);
            this.dwProperties.Guid = new System.Guid("b84b1663-5ada-465a-8fe4-d1f71f2183ec");
            this.dwProperties.Location = new System.Drawing.Point(4, 268);
            this.dwProperties.Name = "dwProperties";
            this.dwProperties.Size = new System.Drawing.Size(263, 80);
            this.dwProperties.TabIndex = 2;
            this.dwProperties.Text = "Properties";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(1, 1);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(261, 78);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // pnlRenderer
            // 
            this.pnlRenderer.BackColor = System.Drawing.SystemColors.Window;
            this.pnlRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRenderer.Location = new System.Drawing.Point(0, 49);
            this.pnlRenderer.Name = "pnlRenderer";
            this.pnlRenderer.Size = new System.Drawing.Size(408, 372);
            this.pnlRenderer.TabIndex = 4;
            this.pnlRenderer.DoubleClick += new System.EventHandler(this.pnlRenderer_DoubleClick);
            this.pnlRenderer.MouseLeave += new System.EventHandler(this.pnlRenderer_MouseLeave);
            this.pnlRenderer.SizeChanged += new System.EventHandler(this.pnlRenderer_SizeChanged);
            this.pnlRenderer.MouseEnter += new System.EventHandler(this.pnlRenderer_MouseEnter);
            // 
            // dockContainer
            // 
            this.dockContainer.AllowDrop = true;
            this.dockContainer.ContentSize = 263;
            this.dockContainer.Controls.Add(this.dwObjects);
            this.dockContainer.Controls.Add(this.dwScene);
            this.dockContainer.Controls.Add(this.dwProperties);
            this.dockContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 400F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwObjects))}, this.dwObjects))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 400F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwScene))}, this.dwScene))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 400F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwProperties))}, this.dwProperties)))});
            this.dockContainer.Location = new System.Drawing.Point(408, 49);
            this.dockContainer.Manager = this.sandDockManager;
            this.dockContainer.Name = "dockContainer";
            this.dockContainer.Size = new System.Drawing.Size(267, 372);
            this.dockContainer.TabIndex = 10;
            // 
            // dwAppearance
            // 
            this.dwAppearance.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwAppearance.Controls.Add(this.tvwRenderer);
            this.dwAppearance.Guid = new System.Guid("40a3d4f5-35eb-4b2a-acec-c3ed52b8f061");
            this.dwAppearance.Location = new System.Drawing.Point(254, 305);
            this.dwAppearance.Name = "dwAppearance";
            this.dwAppearance.Size = new System.Drawing.Size(250, 209);
            this.dwAppearance.TabImage = ((System.Drawing.Image)(resources.GetObject("dwAppearance.TabImage")));
            this.dwAppearance.TabIndex = 0;
            this.dwAppearance.Text = "Theme";
            // 
            // tvwRenderer
            // 
            this.tvwRenderer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvwRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwRenderer.HideSelection = false;
            this.tvwRenderer.LineColor = System.Drawing.Color.Empty;
            this.tvwRenderer.Location = new System.Drawing.Point(1, 1);
            this.tvwRenderer.Name = "tvwRenderer";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Visual Studio 2003";
            treeNode2.Name = "Node11";
            treeNode2.Text = "Luna Blue";
            treeNode3.Name = "Node12";
            treeNode3.Text = "Luna Silver";
            treeNode4.Name = "Node13";
            treeNode4.Text = "Luna Olive";
            treeNode5.Name = "Node14";
            treeNode5.Text = "Classic";
            treeNode6.Name = "Node1";
            treeNode6.Text = "Office 2003";
            treeNode7.Name = "Node15";
            treeNode7.Text = "Luna Blue";
            treeNode8.Name = "Node16";
            treeNode8.Text = "Luna Silver";
            treeNode9.Name = "Node17";
            treeNode9.Text = "Luna Olive";
            treeNode10.Name = "Node18";
            treeNode10.Text = "Classic";
            treeNode11.Name = "Node2";
            treeNode11.Text = "Visual Studio 2005";
            treeNode12.Name = "Node4";
            treeNode12.Text = "Blue";
            treeNode13.Name = "Node5";
            treeNode13.Text = "Black";
            treeNode14.Name = "Node6";
            treeNode14.Text = "Silver";
            treeNode15.Name = "Node3";
            treeNode15.Text = "Office 2007";
            this.tvwRenderer.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode6,
            treeNode11,
            treeNode15});
            this.tvwRenderer.Size = new System.Drawing.Size(248, 207);
            this.tvwRenderer.TabIndex = 4;
            // 
            // dwNewWindows
            // 
            this.dwNewWindows.BackColor = System.Drawing.SystemColors.Window;
            this.dwNewWindows.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwNewWindows.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dwNewWindows.Guid = new System.Guid("1c062ea5-0deb-4b2e-a36a-1564e1520d5f");
            this.dwNewWindows.Location = new System.Drawing.Point(4, 18);
            this.dwNewWindows.Name = "dwNewWindows";
            this.dwNewWindows.Size = new System.Drawing.Size(250, 106);
            this.dwNewWindows.TabImage = ((System.Drawing.Image)(resources.GetObject("dwNewWindows.TabImage")));
            this.dwNewWindows.TabIndex = 1;
            this.dwNewWindows.Text = "Properties";
            // 
            // dwBehavior
            // 
            this.dwBehavior.BackColor = System.Drawing.SystemColors.Window;
            this.dwBehavior.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwBehavior.Controls.Add(this.chkIntegralClose);
            this.dwBehavior.Controls.Add(this.chkAllowOptions);
            this.dwBehavior.Controls.Add(this.chkShowActiveFilesList);
            this.dwBehavior.Controls.Add(this.chkAllowDockContainerResize);
            this.dwBehavior.Controls.Add(this.chkAllowClose);
            this.dwBehavior.Controls.Add(this.chkAllowPin);
            this.dwBehavior.Guid = new System.Guid("6144385b-ee41-4bff-814d-2ba7d497d492");
            this.dwBehavior.Location = new System.Drawing.Point(4, 170);
            this.dwBehavior.Name = "dwBehavior";
            this.dwBehavior.Size = new System.Drawing.Size(250, 111);
            this.dwBehavior.TabImage = ((System.Drawing.Image)(resources.GetObject("dwBehavior.TabImage")));
            this.dwBehavior.TabIndex = 2;
            this.dwBehavior.Text = "Behavior";
            // 
            // chkIntegralClose
            // 
            this.chkIntegralClose.AutoSize = true;
            this.chkIntegralClose.Location = new System.Drawing.Point(12, 132);
            this.chkIntegralClose.Name = "chkIntegralClose";
            this.chkIntegralClose.Size = new System.Drawing.Size(151, 17);
            this.chkIntegralClose.TabIndex = 3;
            this.chkIntegralClose.Text = "Integral Tab Close Buttons";
            // 
            // chkAllowOptions
            // 
            this.chkAllowOptions.Checked = true;
            this.chkAllowOptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowOptions.Location = new System.Drawing.Point(12, 11);
            this.chkAllowOptions.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowOptions.Name = "chkAllowOptions";
            this.chkAllowOptions.Size = new System.Drawing.Size(160, 18);
            this.chkAllowOptions.TabIndex = 3;
            this.chkAllowOptions.Text = "Allow Options Menu";
            // 
            // chkShowActiveFilesList
            // 
            this.chkShowActiveFilesList.AutoSize = true;
            this.chkShowActiveFilesList.Location = new System.Drawing.Point(12, 109);
            this.chkShowActiveFilesList.Name = "chkShowActiveFilesList";
            this.chkShowActiveFilesList.Size = new System.Drawing.Size(129, 17);
            this.chkShowActiveFilesList.TabIndex = 3;
            this.chkShowActiveFilesList.Text = "Show Active Files List";
            // 
            // chkAllowDockContainerResize
            // 
            this.chkAllowDockContainerResize.Checked = true;
            this.chkAllowDockContainerResize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowDockContainerResize.Location = new System.Drawing.Point(12, 77);
            this.chkAllowDockContainerResize.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowDockContainerResize.Name = "chkAllowDockContainerResize";
            this.chkAllowDockContainerResize.Size = new System.Drawing.Size(207, 18);
            this.chkAllowDockContainerResize.TabIndex = 3;
            this.chkAllowDockContainerResize.Text = "Allow Resizing of Docked Windows";
            // 
            // chkAllowClose
            // 
            this.chkAllowClose.Checked = true;
            this.chkAllowClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowClose.Location = new System.Drawing.Point(12, 55);
            this.chkAllowClose.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowClose.Name = "chkAllowClose";
            this.chkAllowClose.Size = new System.Drawing.Size(160, 18);
            this.chkAllowClose.TabIndex = 3;
            this.chkAllowClose.Text = "Allow Closing of Windows";
            // 
            // chkAllowPin
            // 
            this.chkAllowPin.Checked = true;
            this.chkAllowPin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowPin.Location = new System.Drawing.Point(12, 33);
            this.chkAllowPin.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowPin.Name = "chkAllowPin";
            this.chkAllowPin.Size = new System.Drawing.Size(160, 18);
            this.chkAllowPin.TabIndex = 3;
            this.chkAllowPin.Text = "Allow Pinning of Windows";
            // 
            // dwClassView
            // 
            this.dwClassView.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dwClassView.Controls.Add(this.textBox2);
            this.dwClassView.Guid = new System.Guid("2f4de550-af67-4685-8728-5087e47eff30");
            this.dwClassView.Location = new System.Drawing.Point(254, 305);
            this.dwClassView.Name = "dwClassView";
            this.dwClassView.Size = new System.Drawing.Size(250, 111);
            this.dwClassView.TabImage = ((System.Drawing.Image)(resources.GetObject("dwClassView.TabImage")));
            this.dwClassView.TabIndex = 3;
            this.dwClassView.Text = "Class View";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(1, 1);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(248, 109);
            this.textBox2.TabIndex = 1;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 421);
            this.Controls.Add(this.pnlRenderer);
            this.Controls.Add(this.dockContainer);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sandbox";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.dwObjects.ResumeLayout(false);
            this.dwObjects.PerformLayout();
            this.objToolStrip.ResumeLayout(false);
            this.objToolStrip.PerformLayout();
            this.dwScene.ResumeLayout(false);
            this.cmsSceneObjects.ResumeLayout(false);
            this.dwProperties.ResumeLayout(false);
            this.dockContainer.ResumeLayout(false);
            this.dwAppearance.ResumeLayout(false);
            this.dwBehavior.ResumeLayout(false);
            this.dwBehavior.PerformLayout();
            this.dwClassView.ResumeLayout(false);
            this.dwClassView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnToolDefault;
        private TD.SandDock.SandDockManager sandDockManager;
        private TD.SandDock.DockContainer dockContainer2;
        private TD.SandDock.DockableWindow dwAppearance;
        private System.Windows.Forms.TreeView tvwRenderer;
        private TD.SandDock.DockableWindow dwNewWindows;
        private TD.SandDock.DockableWindow dwBehavior;
        private System.Windows.Forms.CheckBox chkIntegralClose;
        private System.Windows.Forms.CheckBox chkAllowOptions;
        private System.Windows.Forms.CheckBox chkShowActiveFilesList;
        private System.Windows.Forms.CheckBox chkAllowDockContainerResize;
        private System.Windows.Forms.CheckBox chkAllowClose;
        private System.Windows.Forms.CheckBox chkAllowPin;
        private TD.SandDock.DockableWindow dwClassView;
        private System.Windows.Forms.TextBox textBox2;
        private TD.SandDock.DockableWindow dwObjects;
        private TD.SandDock.DockableWindow dwScene;
        private System.Windows.Forms.TreeView tvObjects;
        private TD.SandDock.DockableWindow dwProperties;
        private System.Windows.Forms.Panel pnlRenderer;
        private TD.SandDock.DockContainer dockContainer;
        private System.Windows.Forms.ToolStrip objToolStrip;
        private System.Windows.Forms.ToolStripButton btnRefreshObjList;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStripButton btnRotateX;
        private System.Windows.Forms.ToolStripButton btnRotateY;
        private System.Windows.Forms.ToolStripButton btnRotateZ;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnToolVerticalMove;
        private System.Windows.Forms.ToolStripButton btnToolHorizontalMove;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.TreeView tvSceneObjects;
        private System.Windows.Forms.ContextMenuStrip cmsSceneObjects;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnSnap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnPutObjectOnGround;
        private System.Windows.Forms.ToolStripButton btnRun;
    }
}

