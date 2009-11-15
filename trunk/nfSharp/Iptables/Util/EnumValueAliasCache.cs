// NameCache.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
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
//
//

using System;
using System.Collections;
using System.Collections.Generic;

using NFSharp.Common.Types;

namespace NFSharp.Iptables.Util
{
	/// <summary>
	/// Models a cache of enumeration constants stored by alias name
	/// </summary>
	public class EnumValueAliasCache
	{
		private Dictionary<string, object> data;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public EnumValueAliasCache()
		{
			data = new Dictionary<string, object> ();
		}
		
		/// <summary>
		/// Gets the enumeration constant asociated with an alias name if it exists,
		/// if not returns null
		/// </summary>
		public object this[string name]
		{
			get {
				if(this.data.ContainsKey(name))
					return this.data[name];
				else
					return null;
			}
		}
		
		/// <summary>
		/// Clears the cache
		/// </summary>
		public void Clear()
		{
			this.data.Clear();
		}
		
		/// <summary>
		/// Fills the cache from a enumeration type with Alias attributes set
		/// </summary>
		/// <param name="enumType">
		/// A <see cref="Type"/> of an enumertion that has attributes set to the constants
		/// with the values of the aliases.
		/// </param>
		public void FillFromEnum(Type enumType)
		{
			if(enumType == null)
				throw new ArgumentNullException("enumType");
			
			if(!enumType.IsEnum)
				throw new ArgumentException("enumType","The type is not an enumeration");
			
			this.data.Clear();
	        //We are going to keep in memory the option names as the keys of the 
			//dictionary and the enum constant value as the value. 
			//This will speed up the search.
	        
	        Array arr = Enum.GetValues(enumType);
	        string[] aliases;
	        
	        foreach(object obj in arr)
	        {
	            aliases = AliasUtil.GetAliases(obj);
	            
	            for(int i=0;i<aliases.Length;i++)
	            {
	                data.Add(aliases[i], obj);
	            }
	        }
		}
		
		/// <summary>
		/// Adds an alias and a value.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> alias name
		/// </param>
		/// <param name="enumValue">
		/// A <see cref="System.Object"/> alias value
		/// </param>
		/// <exception cref="ArgumentNullException">If the enumeration type is null</exception>
		/// <exception cref="ArgumentException">If the name is null or empty</exception>
		public void Add(string name, object enumValue)
		{
			if(enumValue == null)
				throw new ArgumentNullException ("enumValue");
			
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException ("The name can't be null or empty");
			
			if(!data.ContainsKey(name))
			{
				data.Add (name, enumValue);
			}
			else
			{
				throw new NetfilterException("The key already exists in the name caché: "+name);
			}
		}
		
		/// <summary>
		/// Removes an alias from the cache
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the alias
		/// </param>
		public void Remove(string name)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException ("The name can't be null or empty");
			
			if(data.ContainsKey(name)) {
				data.Remove(name);
			} else 	{
				throw new NetfilterException("The key doesn't exists in caché: "+name);
			}
		}
		
		/// <summary>
		/// Gets if an alias exists in the cache
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the alias
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> that determines if the alias exists or not
		/// </returns>
		public bool Exists(string name)
		{
			return data.ContainsKey(name);
		}
	}
}
