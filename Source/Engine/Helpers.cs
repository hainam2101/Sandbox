using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using MTV3D65;
using Sandbox;

namespace Engine
{
    public class Helpers
    {
        // File formats.

        #region FileFormat enum

        public enum FileFormat
        {
            X,
            TVM,
            TVA,
            WAV,
            MP3,
            OGG,
            TVP
        }

        #endregion

        #region CommonObject enum

        public enum CommonObject
        {
            SKYBOX,
            SKYSPHERE,
            DIRECTIONAL,
            POINT,
            SPOT,
            WATER,
            LANDSCAPE,
            TRIGGER,
            PARTICLE
        }

        #endregion

        #region ObjectType enum

        public enum ObjectType
        {
            NotDetected,
            StaticMesh,
            AnimatedMesh,
            Sound,
            DirectionalLight,
            PointLight,
            SpotLight,
            SkyBox,
            SkySphere,
            Water,
            Landscape,
            Trigger,
            Particle
        }

        #endregion

        #region Constans
        public const string PROGRAM_NAME = "Sandbox";
        public const string PROGRAM_VERSION = "0.3.3";
        #endregion

        #region Private methods.

        /// <summary>
        /// Creates the data paths.
        /// </summary>
        /// <param name="projectPath">The project path.</param>
        public static void CreateDataPaths(string projectPath)
        {
            CreateDirectory(PATH_DATA);
            CreateDirectory(PATH_OBJECTS);
            CreateDirectory(PATH_TEXTURES);
            CreateDirectory(PATH_SOUNDS);
            CreateDirectory(PATH_SCENES);
        }

        #endregion

        #region Public methods.

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        /// <returns>File extension in string.</returns>
        public static string GetFileExtension(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.X:
                    return FILE_X;
                case FileFormat.TVM:
                    return FILE_TVM;
                case FileFormat.TVA:
                    return FILE_TVA;
                case FileFormat.WAV:
                    return FILE_WAV;
                case FileFormat.MP3:
                    return FILE_MP3;
                case FileFormat.OGG:
                    return FILE_OGG;
                case FileFormat.TVP:
                    return FILE_TVP;
                default:
                    return string.Empty;
            }
        }

        public static string GetCommonObjectExtension(CommonObject commonObject)
        {
            switch (commonObject)
            {
                case CommonObject.DIRECTIONAL:
                    return COMMON_OBECT_DIRECTIONAL;
                case CommonObject.POINT:
                    return COMMON_OBECT_POINT;
                case CommonObject.SPOT:
                    return COMMON_OBECT_SPOT;
                case CommonObject.SKYBOX:
                    return COMMON_OBECT_SKYBOX;
                case CommonObject.SKYSPHERE:
                    return COMMON_OBECT_SKYSPHERE;
                case CommonObject.WATER:
                    return COMMON_OBECT_WATER;
                case CommonObject.LANDSCAPE:
                    return COMMON_OBECT_LANDSCAPE;
                case CommonObject.TRIGGER:
                    return COMMON_OBECT_TRIGGER;
                default:
                    return string.Empty;
            }
        }

        public static ObjectType GetObjectType(string fileName)
        {
            fileName = fileName.Split(new[] { '\\' }).Last().ToUpper();

            if (fileName.Equals(GetCommonObjectExtension(CommonObject.SKYBOX)))
            {
                return ObjectType.SkyBox;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.SKYSPHERE)))
            {
                return ObjectType.SkySphere;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.DIRECTIONAL)))
            {
                return ObjectType.DirectionalLight;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.POINT)))
            {
                return ObjectType.PointLight;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.SPOT)))
            {
                return ObjectType.SpotLight;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.WATER)))
            {
                return ObjectType.Water;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.LANDSCAPE)))
            {
                return ObjectType.Landscape;
            }
            else if (fileName.Equals(GetCommonObjectExtension(CommonObject.TRIGGER)))
            {
                return ObjectType.Trigger;
            }
            else if (fileName.EndsWith(GetFileExtension(FileFormat.TVP)))
            {
                return ObjectType.Particle;
            }
            else if (fileName.EndsWith(GetFileExtension(FileFormat.WAV)) ||
                fileName.EndsWith(GetFileExtension(FileFormat.MP3)) ||
                fileName.EndsWith(GetFileExtension(FileFormat.OGG)))
                return ObjectType.Sound;
            else if (fileName.EndsWith(GetFileExtension(FileFormat.TVM)) ||
                             fileName.EndsWith(GetFileExtension(FileFormat.X)))
                return ObjectType.StaticMesh;
            else if (fileName.EndsWith(GetFileExtension(FileFormat.TVA)))
                return ObjectType.AnimatedMesh;
            else
            {
                //MessageBox.Show(string.Format("Can't detect type for {0}.", fileName));
                return ObjectType.NotDetected;
            }
        }

        public static int GetTextureFromResource(ICore core, Bitmap texture)
        {
            var ds = GetTextureSourceFromResource(core, texture);
            return core.TextureFactory.LoadTexture(ds, texture.ToString(), -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_USE_ALPHA_CHANNEL);
        }

        public static string GetTextureSourceFromResource(ICore core, Bitmap texture)
        {
            var ms = new MemoryStream();
            texture.Save(ms, ImageFormat.Png);
            ms.Seek(0, 0);
            var data = ms.ToArray();
            ms.Dispose();
            texture.Dispose();
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var addr = handle.AddrOfPinnedObject().ToInt32();
            handle.Free();
            return core.Globals.GetDataSourceFromMemory(addr, data.Length - 1);
        }

        public static void SetTextureFromResource(ICore core, TVMesh mesh, Bitmap texture, int groupID)
        {
            var ms = new MemoryStream();
            texture.Save(ms, ImageFormat.Png);
            var data = new byte[ms.Length];
            ms.Seek(0, 0);
            data = ms.ToArray();
            ms.Dispose();
            texture.Dispose();
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            int addr = handle.AddrOfPinnedObject().ToInt32();
            handle.Free();
            string ds = core.Globals.GetDataSourceFromMemory(addr, data.Length - 1);
            int texID = core.TextureFactory.LoadTexture(ds);
            mesh.SetTexture(texID, groupID);
        }

        public static int LoadTextureFromResourceToMemory(ICore core, Bitmap texture)
        {
            var ms = new MemoryStream();
            texture.Save(ms, ImageFormat.Png);
            var data = new byte[ms.Length];
            ms.Seek(0, 0);
            data = ms.ToArray();
            ms.Dispose();
            texture.Dispose();
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            int addr = handle.AddrOfPinnedObject().ToInt32();
            handle.Free();
            string ds = core.Globals.GetDataSourceFromMemory(addr, data.Length - 1);
            int texID = core.TextureFactory.LoadTexture(ds);
            return texID;
        }

        public static int GetDUDVTextureFromResource(ICore core, Bitmap texture)
        {
            var ms = new MemoryStream();
            texture.Save(ms, ImageFormat.Png);
            ms.Seek(0, 0);
            var data = ms.ToArray();
            ms.Dispose();
            texture.Dispose();
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var addr = handle.AddrOfPinnedObject().ToInt32();
            handle.Free();
            var ds = core.Globals.GetDataSourceFromMemory(addr, data.Length - 1);
            return core.TextureFactory.LoadDUDVTexture(ds, texture.ToString(), -1, -1, 25);
        }

        public static bool IsTextureFromMemory(string fileName)
        {
            if (fileName.Contains(TEXTURE_FROM_MEMORY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static string DataTypeToString(DataTypes dataType)
        {
            string rezult = string.Empty;

            switch (dataType)
            {
                case DataTypes.String:
                    rezult = "string";
                    break;
                case DataTypes.Int:
                    rezult = "int";
                    break;
                case DataTypes.Float:
                    rezult = "float";
                    break;
                case DataTypes.Bool:
                    rezult = "bool";
                    break;
                case DataTypes.Vector:
                    rezult = "vector";
                    break;
            }

            return rezult;
        }

        public static DataTypes DataTypeFromString(string dataType)
        {
            DataTypes rezult = DataTypes.String;

            switch (dataType.ToLower().Trim())
            {
                case "string":
                    rezult = DataTypes.String;
                    break;
                case "int":
                    rezult = DataTypes.Int;
                    break;
                case "float":
                    rezult = DataTypes.Float;
                    break;
                case "bool":
                    rezult = DataTypes.Bool;
                    break;
                case "vector":
                    rezult = DataTypes.Vector;
                    break;
            }

            return rezult;
        }

        #endregion

        private const string FILE_MP3 = ".MP3";
        private const string FILE_OGG = ".OGG";
        private const string FILE_TVA = ".TVA";
        private const string FILE_TVM = ".TVM";
        private const string FILE_WAV = ".WAV";
        private const string FILE_X = ".X";
        private const string FILE_TVP = ".TVP";

        private const string COMMON_OBECT_DIRECTIONAL = "DIRECTIONAL";
        private const string COMMON_OBECT_POINT = "POINT";
        private const string COMMON_OBECT_SPOT = "SPOT";
        private const string COMMON_OBECT_SKYBOX = "SKYBOX";
        private const string COMMON_OBECT_SKYSPHERE = "SKYSPHERE";
        private const string COMMON_OBECT_WATER = "WATER";
        private const string COMMON_OBECT_LANDSCAPE = "LANDSCAPE";
        private const string COMMON_OBECT_TRIGGER = "TRIGGER";
        private const string COMMON_OBECT_PARTICLE = "PARTICLE";
        // Textures.
        private const string TEXTURE_FROM_MEMORY = "##MEMORY";
        // Paths.
        public const string PATH_DATA = "Data";
        public const string PATH_OBJECTS = "Data\\Objects";
        public const string PATH_SCENES = "Data\\Scenes";
        public const string PATH_SOUNDS = "Data\\Sounds";
        public const string PATH_TEXTURES = "Data\\Textures";
        // Resources.
        public const string TEXTURE_BLANK = "blank";
        // Sun.
        public const string SUN = "sun";
        // Enums.
        public enum BodyBounding
        {
            Convexhull,
            Box,
            Cylinder,
            Sphere,
            None
        }

        public static void SaveSettings(ProgramSettings settings, string productName)
        {
            try
            {
                string settingsFile = Path.Combine(Application.StartupPath, string.Format("{0}{1}", productName, ".xml"));
                using (var fs = new FileStream(settingsFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var xs = new XmlSerializer(typeof(ProgramSettings));
                    xs.Serialize(fs, settings);
                    fs.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static ProgramSettings LaodSettings(string productName)
        {
            string settingsFile = Path.Combine(Application.StartupPath, string.Format("{0}{1}", productName, ".xml"));
            if (File.Exists(settingsFile))
            {
                try
                {
                    using (var fs = new FileStream(settingsFile, FileMode.Open))
                    {
                        var xs = new XmlSerializer(typeof(ProgramSettings));
                        var settings = (ProgramSettings)xs.Deserialize(fs);
                        fs.Close();
                        return settings;
                    }
                }
                catch (Exception)
                {
                    return new ProgramSettings();
                }
            }
            else
            {
                return new ProgramSettings();
            }
        }
    }
}