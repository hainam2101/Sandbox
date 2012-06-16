using System;
using System.ComponentModel;

namespace Engine
{
    // This is a special type converter which will be associated with the Custom class.
    // It converts an Custom object to string representation for use in a property grid.
    internal class CustomConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is Custom)
            {
                // Cast the value to an Custom type
                Custom emp = (Custom)value;

                // Return department and department role separated by comma.
                return emp.Value;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    // This is a special type converter which will be associated with the CustomCollection class.
    // It converts an CustomCollection object to a string representation for use in a property grid.
    internal class CustomCollectionConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is CustomCollection)
            {
                // Return department and department role separated by comma.
                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

}
