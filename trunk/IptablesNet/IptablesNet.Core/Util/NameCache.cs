
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
				throw new IptablesException("The key already exists in the name caché: "+name);
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
				throw new IptablesException("The key doesn't exists in caché: "+name);
			}
		}
		
		public bool Exists(string name)
		{
			return data.ContainsKey(name);
		}
	}
}
