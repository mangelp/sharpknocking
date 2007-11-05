// AbstractParameters.cs
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
using System.Collections.Generic;

namespace IptablesNet.Core
{

	/// <summary>
	/// Models an abstract parameter. This is the base class for everything that
	/// has a command-line option format
	/// </summary>
	public abstract class AbstractParameter
	{
		/// <summary>
		/// Returns if the parameter is in long format or in short format
		/// </summary>
		public virtual bool IsLongFormat
		{
			get {
				string def = this.GetDefaultAlias();
				
				if(def!= null && def.Length>1)
					return true;
				else
					return false;
			}
		}
		
		protected AbstractParameter ()
		{}
		
		/// <summary>
		/// Checks if the name is a valid alias for this parameter.
		/// </summary>
		public abstract bool IsAlias(string name);
		
		/// <summary>
		/// Gets a default alias name for this parameter.
		/// </summary>
		/// <remarks>
		/// This alias is the default name for the command
		/// </remarks>
		public abstract string GetDefaultAlias();
		
		public override string ToString ()
		{
			string val = this.GetValuesAsString();
			
			if(!String.IsNullOrEmpty (val))
				return this.GetNameAsString ()+" "+val;
			else
				return this.GetNameAsString ();
		}
		
		protected virtual string GetNameAsString(bool longFormat)
		{
		    string result = String.Empty;
		    
		    if(longFormat)
		        result = "--"+this.GetDefaultAlias();
		    else
		        result = "-"+this.GetDefaultAlias();
			
			return result;			
		}
		
		protected string GetNameAsString()
		{
			return this.GetNameAsString( this.IsLongFormat);
		}

		protected abstract string GetValuesAsString();
		
	}
}
