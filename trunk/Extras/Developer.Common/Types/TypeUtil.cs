// project created on 22/01/2007 at 22:22
using System;
using System.Reflection;

namespace Developer.Common.Types
{
	/// <summary>
	/// Utility methods to use with types that have defined the AliasAttribute.
	/// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// Checks if the value is a constant value defined for any of the
        /// enumeration members.
        /// </summary>
        /// <remarks>
        /// The value parameter can be a string with the numeric value  to
        /// check, an string with the name of the enumeration or a number.
        /// </remarks>
		public static bool IsEnumValue(Type enumType, string enumValue)
		{
			object obj;
            return TryGetEnumValue(enumType, enumValue, out obj);		        
		}
		
		/// <summary>
		/// Tries to convert a value to an enumeration const
		/// </summary>
		public static bool TryGetEnumValue(Type enumType, object value, out object enumConst)
		{
			enumConst = null;
		    if(value==null)
		        return false;
			
			if(value is string || enumConst.GetType().IsValueType)
			{
				string strVal = value.ToString();
				try {
					enumConst = Enum.Parse(enumType, strVal, true);
					//Console.WriteLine("Got enum "+enumConst+" from "+value+" as "+value.GetType().Name);
				} catch (Exception ex) {
					//Console.Out.WriteLine("\n-- Exception detected: "+ex.Message+" --\n"+ex+"\n-- End of exception --\n");
					return false;
				}
				return true;
			}
			else if(Enum.IsDefined(enumType, value))
			{
				enumConst = Enum.ToObject(enumType, value);
				Console.WriteLine("Got enum "+enumConst+" from "+value+" as "+value.GetType().Name);
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Returns the enum member that represents the value if it is valid
		/// </summary>
		/// <remarks>
		/// If the value can't be converted to an enum value this returns null.
		/// </remarks>
		public static object GetEnumValue(Type enumType, object value)
		{
		    if(value==null)
		        return null;
					        
		    if(value.GetType() == Enum.GetUnderlyingType(enumType) &&
		                  !Enum.IsDefined(enumType, value)) {
		        return null;
		    }
		    
		    string strVal = value.ToString().Trim();
		    return Enum.Parse(enumType, strVal, true);
		}
		
     
		
        /// <summary>
        /// Returns the attribute if it exists in the custom attributes of the
        /// value constant.
        /// </summary>
        /// <remarks>
        /// The object must be a enumeration constant or this method will die within
        /// an exception.
        /// </remarks>
		public static object GetEnumAttribute(Type attrType, object enumValue)
		{
		    FieldInfo finfo = enumValue.GetType().GetField(""+enumValue);
		    
		    object[] attributes = finfo.GetCustomAttributes(false);
		    
		    foreach (Attribute attr in attributes)
		    {
		        if (attr.GetType() == attrType)
		        {
		            return attr;
		        }
		    }
		    
		    return null;
		}
	}
}