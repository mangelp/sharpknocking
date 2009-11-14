// project created on 22/01/2007 at 22:22
using System;
using System.Reflection;

namespace SharpKnocking.Common
{
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
        /// Returns the first custom attribute of a type
        /// </summary>
        /// <remarks>
        /// If the attribute isn't found returns null or the attribute if exists.
        /// </remarks>
        public static Attribute GetAttribute(Type attrType, object target)
        {
		    object[] attributes = target.GetType().GetCustomAttributes(true);
		    
		    foreach (Attribute attr in attributes)
		    {
		        if (attr.GetType() == attrType)
		        {
		            return attr;
		        }
		    }
		    
		    return null;
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
		
		
		/// <summary>
		/// Searches the custom alias attribute and returns the values of the
		/// aliases
		/// </summary>
		public static string[] GetAliases(object enumValue)
		{
		    
		    object attr = TypeUtil.GetEnumAttribute(typeof(AliasAttribute),
		                                              enumValue);
		    
		    if(attr!=null)
		    {
		        return ((AliasAttribute)attr).Aliases;
		    }
		    
		    return new string[]{};
		}
		
		/// <summary>
		/// Returns a string with the default alias name for the enumeration
		/// constant.
		/// </summary>
		public static string GetDefaultAlias(object enumValue)
		{
		    string[] aliases = TypeUtil.GetAliases(enumValue);
		    
		    if(aliases.Length==0)
		        return String.Empty;
		    else
		        return aliases[aliases.Length-1];
		}
		
		/// <summary>
		/// Gets if a name is an alias for a enum value.
		/// </summary>
		public static bool IsAliasName(object enumValue, string name)
		{
		    string [] aliases = TypeUtil.GetAliases(enumValue);
		    name = name.ToLower();
		    
		    for(int i=0;i<aliases.Length; i++)
		    {
		        if(aliases[i].ToLower().Equals(name))
		            return true;    
		    }
		    
		    return false;
		}
		
        /// <summary>
        /// Checks if the name is an alias name for a enumeration type where the
        /// constant have the custom attribute AliasAttribute.
        /// </summary>
        /// <remarks>
        /// If the constants doesn't have the attribute or if it is not found this
        /// returns false. Otherwise returns true
        /// </remarks>
		public static bool IsAliasName(Type enumType, string name, out object value)
		{
		    value = null;
		    Array values = Enum.GetValues(enumType);
		    
		    for(int i=0;i<values.Length;i++)
		    {
		        if(TypeUtil.IsAliasName(values.GetValue(i), name))
		        {
		            value = values.GetValue(i);
		            return true;
		        }
		    }
		    
		    return false;
		}
	}
}