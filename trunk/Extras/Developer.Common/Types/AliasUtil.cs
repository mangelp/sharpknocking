// /home/mangelp/Projects/sharpknocking/Extras/Developer.Common/Type/AliasUtil.cs created with MonoDevelop at 14:32 14/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;

namespace Developer.Common.Types
{	
	public static class AliasUtil
	{
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
		    string[] aliases = AliasUtil.GetAliases(enumValue);
		    
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
		    string [] aliases = AliasUtil.GetAliases(enumValue);
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
		        if(AliasUtil.IsAliasName(values.GetValue(i), name))
		        {
		            value = values.GetValue(i);
		            return true;
		        }
		    }
		    
		    return false;
		}
	}
}
