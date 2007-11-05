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

using Developer.Common.Types;

namespace IptablesNet.Core.Util
{
	
	public class NameCache
	{
		private Dictionary<string, object> data;
		
		public NameCache()
		{
			data = new Dictionary<string, object> ();
		}
		
		public object this[string name]
		{
			get {
				if(this.data.ContainsKey(name))
					return this.data[name];
				else
					return null;
			}
		}
		
		public void Clear()
		{
			this.data.Clear();
		}
		
		public void FillFromEnum(Type enumType)
		{
			this.data.Clear();
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
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
		
		public void Remove(string name)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException ("The name can't be null or empty");
			
			if(data.ContainsKey(name))
			{
				data.Remove(name);
			}
			else
			{
				throw new NetfilterException("The key doesn't exists in caché: "+name);
			}
		}
		
		public bool Exists(string name)
		{
			return data.ContainsKey(name);
		}
	}
}
