using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using MTV3D65;

namespace Engine
{
    [TypeConverter(typeof (VECTOR3DConverter))]
    public struct VECTOR3D
    {
        private float x;
        private float y;
        private float z;

        public VECTOR3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public VECTOR3D(TV_3DVECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }

        public TV_3DVECTOR GetTv3DVector()
        {
            return new TV_3DVECTOR(x, y, z);
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }
    }

    public class VECTOR3DConverter : TypeConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new VECTOR3D((float) propertyValues["X"], (float) propertyValues["Y"], (float) propertyValues["Z"]);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);

            var sortOrder = new string[3];

            sortOrder[0] = "X";
            sortOrder[1] = "Y";
            sortOrder[2] = "Z";

            return properties.Sort(sortOrder);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            bool canConvert = (sourceType == typeof (string));

            if (!canConvert)
                canConvert = CanConvertFrom(context, sourceType);

            return canConvert;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var sValue = value as string;
            object retVal = null;

            if (sValue != null)
            {
                sValue = sValue.Trim();

                if (sValue.Length != 0)
                {
                    if (null == culture)
                        culture = CultureInfo.CurrentCulture;

                    string[] parms = sValue.Split(new char[] {culture.TextInfo.ListSeparator[0]});

                    if (parms.Length == 1)
                    {
                        float x = float.Parse(parms[0]);
                        retVal = new VECTOR3D(x, x, x);
                    }
                    else if (parms.Length == 3)
                    {
                        float x = float.Parse(parms[0]);
                        float y = float.Parse(parms[1]);
                        float z = float.Parse(parms[2]);

                        retVal = new VECTOR3D(x, y, z);
                    }
                }
            }
            else
                retVal = ConvertFrom(context, culture, value);

            return retVal;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            bool canConvert = (destinationType == typeof (InstanceDescriptor));

            if (!canConvert)
                canConvert = base.CanConvertFrom(context, destinationType);

            return canConvert;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            object retVal = null;
            var VECTOR3D = (VECTOR3D) value;

            if (destinationType == typeof (InstanceDescriptor))
            {
                var argTypes = new Type[3];

                argTypes[0] = typeof (float);
                argTypes[1] = typeof (float);
                argTypes[2] = typeof (float);

                ConstructorInfo constructor = typeof (VECTOR3D).GetConstructor(argTypes);

                var arguments = new object[3];

                arguments[0] = VECTOR3D.X;
                arguments[1] = VECTOR3D.Y;
                arguments[2] = VECTOR3D.Z;

                retVal = new InstanceDescriptor(constructor, arguments);
            }
            else if (destinationType == typeof (string))
            {
                if (null == culture)
                    culture = CultureInfo.CurrentCulture;

                var values = new string[3];

                TypeConverter numberConverter = TypeDescriptor.GetConverter(typeof (float));

                values[0] = VECTOR3D.X.ToString();
                values[1] = VECTOR3D.Y.ToString();
                values[2] = VECTOR3D.Z.ToString();

                retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
            }
            else
                retVal = base.ConvertTo(context, culture, value, destinationType);

            return retVal;
        }
    }
}