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
	/// Models an abstract parameter and generic operations for parameters
	/// </summary>
	/// <remarks>
	/// This is the base class for everything that has a command-line option format of
	/// a name followed by a value.
	/// This class doesn't specify a property for the name and the value and only provides
	/// methods to get them.
	/// </remarks>
	public abstract class AbstractParameter
	{
		/// <summary>
		/// Returns if the parameter is in long format or in short format
		/// </summary>
		/// <remarks>
		/// By default the value will be in long format if the default name of the
		/// parameter is longer than one character.
		/// </remarks>
		/// <returns>
		/// True if the default alias name is longer than one character or false if it is
		/// null or has only one character.
		/// This property must be overriden by inheritors to specify a concrete behaviour.
		/// </returns>
		public virtual bool IsLongFormat
		{
			get {
				string def = this.GetDefaultAlias();
				
				if(!String.IsNullOrEmpty(def) && def.Length > 1)
					return true;
				else
					return false;
			}
		}
		
		/// <summary>
		/// Base constructor for the abstract class. Evidently can't be called directly
		/// </summary>
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
		/// This name is used when converting the object to an string.
		/// </remarks>
		public abstract string GetDefaultAlias();
		
		/// <summary>
		/// Converts the object to an string that represents it
		/// </summary>
		/// <remarks>
		/// This string could be parsed back to the object.
		/// </remarks>
		/// <returns>
		/// A <see cref="System.String"/> that represents the object's values
		/// </returns>
		public override string ToString ()
		{
			string val = this.GetValueAsString();
			
			if(! String.IsNullOrEmpty(val))
				return this.GetNameAsString () + " " + val;
			else
				return this.GetNameAsString ();
		}
		
		/// <summary>
		/// Gets an string with the name of the object with the option characters '-'.
		/// </summary>
		/// <param name="longFormat">
		/// A <see cref="System.Boolean"/> that specifies if it should be got as a long
		/// format option or not.
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> with the name of the object with the option
		/// characters '-'.
		/// </returns>
		protected virtual string GetNameAsString(bool longFormat)
		{
		    string result = String.Empty;
		    
		    if(longFormat)
		        result = "--" + this.GetDefaultAlias();
		    else
		        result = "-" + this.GetDefaultAlias();
			
			return result;			
		}
		
		/// <summary>
		/// Gets a string with the name of the parameter
		/// </summary>
		/// <remarks>
		/// By default this method returns the long format option.
		/// </remarks>
		/// <returns>
		/// A <see cref="System.String"/> with the name of the parameter
		/// </returns>
		protected string GetNameAsString()
		{
			return this.GetNameAsString(this.IsLongFormat);
		}
		
		/// <summary>
		/// Gets an string with the values of this parameter
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> with the values if this parameter
		/// </returns>
		protected abstract string GetValueAsString();		
	}
}
