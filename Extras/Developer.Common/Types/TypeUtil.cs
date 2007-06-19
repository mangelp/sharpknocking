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
        /// check, a string with the name of the enumeration or a number.
        /// </remarks>
		public static bool IsEnumValue(Type enumType, string enumValue)
		{
            return (TypeUtil.GetEnumValue(enumType, enumValue)!=null);		        
		}
		
		/// <summary>
		/// Returns the enum member that represents the value if it is valid
		/// </summary>
		public static object GetEnumValue(Type enumType, object value)
		{
		    if(value==null)
		        return null;
		        
		    if(value.GetType() == Enum.GetUnderlyingType(enumType) &&
		                  !Enum.IsDefined(enumType, value))
		    {
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