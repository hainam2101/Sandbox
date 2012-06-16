using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Engine;
using ScintillaNet;

namespace Sandbox
{
    [Serializable]
    public class ProgramSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramSettings"/> class.
        /// </summary>
        public ProgramSettings()
        {
            // Defaults.
            CameraMoveSpeed = 0.5f;
            CameraMouseSpeed = 0.05f;
            CameraRunSpeed = 3f;
            CameraScrollSpeed = 0.05f;
            SnapToGrid = false;
            SnapValue = 1f;
            UseAdvanceTexturing = true;
            TextureNormalSuffix = "_n";
            TextureSpecularSuffix = "_s";
            TextureHeightSuffix = "_h";
            Antialiasing = _Antialiasing.None;
            VSync = true;
            RotationValue = 45.0f;
            BackColor = new MyColor(Color.LightGray.R, Color.LightGray.G, Color.LightGray.B);
            Script = string.Empty;
            ScriptFileExtension = string.Empty;
            Shadows = false;
            ScriptLanguage = Lexer.Lua;
        }

        [Category("Rendering")]
        [DisplayName("Background color")]
        [Description("Sets editor background color.")]
        [EditorAttribute(typeof(MyColorEditor), typeof(UITypeEditor))]
        public MyColor BackColor { get; set; }

        [Category("Rendering")]
        [DisplayName("Use advance texturing")]
        [Description("Determines whether to use advanced texturing using normal, specular and height textures.")]
        public bool UseAdvanceTexturing { get; set; }

        [Category("Rendering")]
        [DisplayName("VSync")]
        [Description("Determines whether to set VSync.")]
        public bool VSync { get; set; }

        [Category("Base")]
        [DisplayName("Normal suffix")]
        [Description("Normal texture suffix.")]
        public string TextureNormalSuffix { get; set; }

        [Category("Base")]
        [DisplayName("Specular suffix")]
        [Description("Specular texture suffix.")]
        public string TextureSpecularSuffix { get; set; }

        [Category("Base")]
        [DisplayName("Height suffix")]
        [Description("Height texture suffix.")]
        public string TextureHeightSuffix { get; set; }

        public enum _Antialiasing
        {
            None,
            X2,
            X4,
            X6,
            X8,
            X10,
            X12,
            X14,
            X16
        }

        [Category("Rendering")]
        [DisplayName("Antialiasing")]
        [Description("Determines which anti aliasing level to use. Restart program after change.")]
        public _Antialiasing Antialiasing { get; set; }

        [Category("Rendering")]
        [DisplayName("Shadows")]
        [Description("Switch stencil shadows on or off.")]
        public bool Shadows { get; set; }

        [Category("Camera")]
        [DisplayName("Navigating speed")]
        [Description("Navigating speed using WASDQR keys.")]
        public float CameraMoveSpeed { get; set; }

        [Category("Camera")]
        [DisplayName("Mouse speed")]
        [Description("Mouse speed.")]
        public float CameraMouseSpeed { get; set; }

        [Category("Camera")]
        [DisplayName("Faster navigating speed")]
        [Description("Navigating speed when shift key is pressed (navigating speed * this value).")]
        public float CameraRunSpeed { get; set; }

        [Category("Camera")]
        [DisplayName("Mouse scroll speed")]
        [Description("Mouse scroll speed.")]
        public float CameraScrollSpeed { get; set; }
        
        // Snap.
        [Category("Snap")]
        [DisplayName("Snap to grid")]
        [Description("Determines whether object movement snaps to grid.")]
        public bool SnapToGrid { get; set; }

        [Category("Snap")]
        [DisplayName("Snap value")]
        [Description("Snap every referred value.")]
        public float SnapValue { get; set; }

        [Category("Navigation")]
        [DisplayName("Rotation angle")]
        [Description("Rotation angle when rotation tool is used.")]
        public float RotationValue { get; set; }

        [Category("Run")]
        [DisplayName("Run program")]
        [Description("Describes which program to run when F5 key is hit.")]
        [System.ComponentModel.Editor(typeof(System.Windows.Forms.Design.FileNameEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public string ProgramToRun { get; set; }

        [Category("Script"), DisplayName("Enabled")]
        [Description("Defines if script by default is enabled.")]
        public bool ScriptEnabled { get; set; }

        [Category("Script"), DisplayName("Language")]
        [Description("Defines scripting language. This value is used for syntax highlighting.")]
        public Lexer ScriptLanguage { get; set; }

        [Category("Script"), DisplayName("Script stub")]
        [Description("Defines script stub.")]
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
        public string Script { get; set; }

        [Category("Script"), DisplayName("Script file extension")]
        [Description("Defines what script files must be visible in object tree. Enter file extension.")]
        public string ScriptFileExtension { get; set; }
    }
}