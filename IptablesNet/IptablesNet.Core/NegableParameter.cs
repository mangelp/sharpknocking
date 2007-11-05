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

namespace IptablesNet.Core
{
	public abstract class NegableParameter: AbstractParameter
	{
	    private bool not;
	    
	    /// <summary>
	    /// Gets/sets if the meaning of the match is interpreted as the opposite.
	    /// This will not apply to every parameter.
	    /// </summary>
	    public bool Not
	    {
	        get { return this.not;}
	        set { this.not = value;}
	    }
		
		public NegableParameter()
			:base()
		{
		}
		
		/// <summary>
		/// Returns an string that represents the parameter that models this class.
		/// </summary>
		/// <remarks>
		/// This method uses only name and value (along with IsLongOption and Not
        /// properties values) to build the string.
        /// every class that extends this class should keep these fields updated
        /// to keep this working.
		/// </remarks>
		public override string ToString()
		{
		    string result = base.GetNameAsString();
			
			string value = this.GetValuesAsString ();
			
		    if(this.not)
		    {
		        if(String.IsNullOrEmpty(value))
		            result = "! "+result;
		        else
		            result += " ! ";
		    }
		    else if(!String.IsNullOrEmpty(value))
		    {
		        result+=" "+this.GetValuesAsString();
		    }
			
			//Console.WriteLine("Writing string for: "+this.GetNameAsString()+"["+result+"]");
		    
		    return result;
		}
	}
}
