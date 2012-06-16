using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using MTV3D65;

namespace Engine
{
    [TypeConverter(typeof (UVConverter))]
    public struct UV
    {
        private float u;
        private float v;

        public UV(float u, float v)
        {
            this.u = u;
            this.v = v;
        }

        public TV_2DVECTOR GetTUV()
        {
            return new TV_2DVECTOR(u, v);
        }

        public float U
        {
            get { return u; }
            set { u = value; }
        }

        public float V
        {
            get { return v; }
            set { v = value; }
        }
    }

    public class UVConverter : TypeConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new UV((float) propertyValues["U"], (float) propertyValues["V"]);
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

            sortOrder[0] = "U";
            sortOrder[1] = "V";

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
                        float u = float.Parse(parms[0]);
                        retVal = new UV(u, u);
                    }
                    else if (parms.Length == 2)
                    {
                        float u = float.Parse(parms[0]);
                        float v = float.Parse(parms[1]);

                        retVal = new UV(u, v);
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
            var UV = (UV) value;

            if (destinationType == typeof (InstanceDescriptor))
            {
                var argTypes = new Type[2];

                argTypes[0] = typeof (float);
                argTypes[1] = typeof (float);

                ConstructorInfo constructor = typeof (UV).GetConstructor(argTypes);

                var arguments = new object[2];

                arguments[0] = UV.U;
                arguments[1] = UV.V;

                retVal = new InstanceDescriptor(constructor, arguments);
            }
            else if (destinationType == typeof (string))
            {
                if (null == culture)
                    culture = CultureInfo.CurrentCulture;

                var values = new string[2];

                TypeConverter numberConverter = TypeDescriptor.GetConverter(typeof (float));

                values[0] = UV.U.ToString();
                values[1] = UV.V.ToString();

                retVal = String.Join(culture.TextInfo.ListSeparator + " ", values);
            }
            else
                retVal = base.ConvertTo(context, culture, value, destinationType);

            return retVal;
        }
    }
}