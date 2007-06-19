
using System;
using System.Collections;
using System.Collections.Generic;

using Developer.Common.Types;

namespace IptablesNet.Core.Extensions
{
	public class ReadOnlyListAdapter<T>
		where T:class,IExtensionHandler
	{
		private List<T> adapted;
		
	    /// Returns the extension handler if it exists.
	    /// </summary>
	    /// <param name="extension">Enumeration constant that represents the
	    /// extension to return</param>
	    /// <returns>
	    /// The extension object if it exists or null if not
	    /// </returns>
	    public T this[object extType]
	    {
	        get
	        {
				foreach( T handler in this.adapted)
				{
					if(AliasUtil.IsAliasName (extType, handler.ExtensionName))
						return handler;
				}
				
				return null;
	        }
	    }
	    
	    public T this[string name]
	    {
	        get
	        {
				foreach( T handler in this.adapted)
				{
					if(handler.ExtensionName.Equals(name,StringComparison.InvariantCultureIgnoreCase))
						return handler;
				}
				
				return null;
	        }
	    }
	    
	    public T this[int index]
	    {
	        get
	        {
	            return this.adapted[index];
	        }
	    }
	    
	    public int Count
	    {
	        get { return this.adapted.Count;}    
	    }
	    
		public ReadOnlyListAdapter(List<T> adapted)
		{
		    this.adapted = adapted;
		}
	}
}
