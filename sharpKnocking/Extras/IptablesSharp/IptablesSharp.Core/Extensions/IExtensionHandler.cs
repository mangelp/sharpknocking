// IExtensionHandler.cs
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

namespace IptablesSharp.Core.Extensions
{
	public interface IExtensionHandler
	{
	    // ------------------------------------------------------------- //
	    // Instance properties                                           //
	    // ------------------------------------------------------------- //
	    
		/// <summary>
		/// Gets the name for the extension
		/// </summary>
	    string ExtensionName { get; }
	    
	    // ------------------------------------------------------------- //
	    // Instance methods                                              //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Checks if exists a parameter with the given name. When checking the names
		/// is also checked the aliases for each parameter.
	    /// </summary>
	    bool Contains(string name);

	    /// <summary>
	    /// Adds a parameter. The parameter is converted to other type if needed
	    /// so the original parameter is not added (a copy is added) in
	    /// the collection.
	    /// </summary>
	    /// <remarks>
	    /// Before adding first checks that is a valid parameter and then does
	    /// a check over the parameter to know if it matches the internal type
	    /// used by the extension for the parameters or not.<br/>
	    /// If the type doesn't match a new parameter instance is created and
	    /// then assigned to the internal collection.<br/>
	    /// Due to this it's a bad idea to keep a instance of the parameter added
	    /// and it's best to look for the parameter before touching it.
	    /// </remarks>
	    void AddParameter(string name, string value);
	    
	    /// <summary>
	    /// Removes a parameter from the extension.
	    /// </summary>
	    void RemoveParameter(string name);
	    
	    /// <summary>
	    /// Checks if the name is a valid parameter name for this extension
	    /// </summary>
	    bool IsSupportedParam(string name);
	}
}
