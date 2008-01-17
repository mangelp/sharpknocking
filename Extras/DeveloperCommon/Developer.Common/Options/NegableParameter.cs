// NegableParameter.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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

namespace Developer.Common.Options
{
	/// <summary>
	/// Models a parameter that can be preceeded by an '!' character that changes the 
	/// sense of the value.
	/// </summary>
	public abstract class NegableParameter: AbstractParameter
	{
	    private bool not;
	    
	    /// <summary>
	    /// Gets/sets if the meaning of the value must change
	    /// </summary>
	    public bool Not
	    {
	        get { return this.not;}
	        set { this.not = value;}
	    }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NegableParameter()
			:base()
		{
		}
		
		/// <summary>
		/// Returns an string that represents the parameter
		/// </summary>
		/// <remarks>
		/// Refines parent default implementation.
		/// </remarks>
		public override string ToString()
		{
		    string result = base.GetNameAsString();
			string value = this.GetValueAsString ();
			
		    if (this.not) {
		        if (String.IsNullOrEmpty(value))
		            result = "! "+result;
				else
					result = "! "+result+" "+value;
		    } else if (!String.IsNullOrEmpty(value)) {
		        result += " " + value;
		    }
		    
		    return result;
		}
	}
}
