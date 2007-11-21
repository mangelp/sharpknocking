// AliasUtil.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 


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
