// AliasAttribute.cs
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
	
	/// <summary>
	/// Meta information about an alternate name for an enumeration member.
	/// </summary>
	/// <remarks>
	/// This atribute can check if a string match with any name available
	/// </remarks>
	public class AliasAttribute: Attribute
	{
	    private string[] aliases;
	    
		/// <summary>
		/// Aliases for this attribute name
		/// </summary>
	    public string[] Aliases
	    {
	        get {return this.aliases;}    
	    }
	    
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aliases">
		/// A <see cref="System.String"/>
		/// </param>
		public AliasAttribute(params string[] aliases)
		  :base()
		{
			this.aliases = aliases;
		}
		
		/// <summary>
		/// Checks if the name match any of the aliases
		/// </summary>
		public bool Match(string name, bool caseSensitive)
		{
		    if(name==null || name.Length==0)
		        return false;
		    
		    if(!caseSensitive)
		        name = name.ToLower();
		    
		    foreach(string alias in this.aliases)
		    {
		        if(caseSensitive && name.Equals(alias))
		        {
		            return true;
		        }
		        else if(!caseSensitive && name.Equals(alias.ToLower()))
                {
                    return true;                                  
                }
		    }
		    
		    return false;
		}
	}
}
