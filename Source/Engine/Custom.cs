using System.ComponentModel;

namespace Engine
{
    public enum DataTypes
    {
        String,
        Int,
        Float,
        Bool,
        Vector
    }

	/// <summary>
	/// Person is the test class defining two properties: first name and last name .
	/// By deriving from GlobalizedObject the displaying of property names are language aware.
	/// GlobalizedObject implements the interface ICustomTypeDescriptor. 
	/// </summary>
    [TypeConverter(typeof(CustomConverter))]
	public class Custom
	{
		private string name = "";
		private string value = "";
        //private DataTypes dataType = DataTypes.String;

		public Custom() {}

        public Custom(string name, /*DataTypes dataType,*/ string value)
        {
            Name = name;
            //DataType = dataType;
            Value = value;
        }

		[Category("Custom")]
		public string Name
		{
			get { return name; }
            set { name = value; }
		}

        [Category("Custom")]
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}

        //[Category("Custom")]
        //public DataTypes DataType
        //{
        //    get { return dataType; }
        //    set { dataType = value; }
        //}      
	}
}

