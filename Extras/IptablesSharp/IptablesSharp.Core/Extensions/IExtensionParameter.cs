// IExtensionParameter.cs
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

namespace IptablesNet.Core
{	
	public interface IExtensionParameter
	{
		void SetValues (string value);
		
		/// <summary>
		/// Parses the value string and fills the properties of the parameter.
		/// </summary>
		/// <remarks>
		/// This method must be implemented and throw FormatException when the
		/// string cannot be parsed
		/// </remarks>
		bool TrySetValues (string value, out string errMsg);
		
		/// <summary>
		/// Returns the default name for the parameter
		/// </summary>
		string GetDefaultAlias ();

		/// <summary>
		/// Returns if a name is a valid alias for the parameter
		/// </summary>
		bool IsAlias (string name);
	}
}
