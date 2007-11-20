// ReadOnlyListAdapter.cs
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

namespace IptablesNet.Core.Extensions
{
	/// <summary>
	/// Read-only adapter for lists. This adapter allows executing operations over a list
	/// without allowing to modify it.
	/// </summary>
	public class ReadOnlyListAdapter<T>
		where T:class,IExtensionHandler
	{
		private List<T> adapted;
		
		/// <summary>
	    /// Returns the extension handler if it exists.
	    /// </summary>
	    /// <param name="extType">Enumeration constant that represents the
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

		/// <summary>
		/// Returns the extension handler if it exists.
		/// </summary>
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
	    
		/// <summary>
		/// Returns the number of elements that this adapter can access.
		/// </summary>
		/// <param name="adapted">
		/// A <see cref="List`1"/>
		/// </param>
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
