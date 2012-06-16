using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using MTV3D65;

namespace Engine
{
    [TypeConverter(typeof (VECTOR2DConverter))]
    public struct VECTOR2D
    {
        private float x;
        private float y;

        public VECTOR2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public TV_2DVECTOR GetTv2DVector()
        {
            return new TV_2DVECTOR(x, y);
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
    }

    public class VECTOR2DConverter : TypeConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new VECTOR2D((float) propertyValues["X"], (float) propertyValues["Y"]);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);

            var sortOrder = new string[2];

            sortOrder[0] = "X";
            sortOrder[1] = "Y";

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
                        retVal = new VECTOR2D(x, x);
                    }
                    else if (parms.Length == 2)
                    {
                        float x = float.Parse(parms[0]);
                        float y = float.Parse(parms[1]);

                        retVal = new VECTOR2D(x, y);
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
            var VECTOR2D = (VECTOR2D) value;

            if (destinationType == typeof (InstanceDescriptor))
            {
                var argTypes = new Type[2];

                argTypes[0] = typeof (float);
                argTypes[1] = typeof (float);

                ConstructorInfo constructor = typeof (VECTOR2D).GetConstructor(argTypes);

                var arguments = new object[2];

                arguments[0] = VECTOR2D.X;
                arguments[1] = VECTOR2D.Y;

                retVal = new InstanceDescriptor(constructor, arguments);
            }
            else if (destinationType == typeof (string))
            {
                if (null == culture)
                    culture = CultureInfo.CurrentCulture;

                var values = new string[2];

                TypeConverter numberConverter = TypeDescriptor.GetConverter(typeof (float));

                values[0] = VECTOR2D.X.ToString();
                values[1] = VECTOR2D.Y.ToString();

                retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
            }
            else
                retVal = base.ConvertTo(context, culture, value, destinationType);

            return retVal;
        }
    }
}