// IStringConvertible.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{aT}gmail[D0T]com)
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
using System.Text;

namespace IptablesNet.Core
{
	/// <summary>
	/// Models methods to convert a object to an string that can be parsed back by the 
	/// object type into an object's instance.
	/// </summary>
	public interface IStringConvertibleObject
	{
		/// <summary>
		/// Gets an string with all the contents of the object
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		string GetContentsAsString();
		
		/// <summary>
		/// Gets an string with all the contents of the object using a format
		/// </summary>
		/// <param name="options">
		/// A <see cref="StringFormatOptions"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> formatted
		/// </returns>
		string GetContentsAsString(StringFormatOptions options);
		
		/// <summary>
		/// Appends the contents of the object to an string 
		/// </summary>
		/// <remarks>
		/// This method can also append other information required for a concrete object.
		/// </remarks>
		/// <param name="sb">
		/// A <see cref="StringBuilder"/> where the contents of the object will be appended
		/// as strings.
		/// </param>
		void AppendTo(StringBuilder sb);
		
		/// <summary>
		/// Appends the contents of the object to an string using a format
		/// </summary>
		/// <param name="sb">
		/// A <see cref="StringBuilder"/>
		/// </param>
		/// <param name="options">
		/// A <see cref="StringFormatOptions"/> with the format to use
		/// </param>
		void AppendTo(StringBuilder sb, StringFormatOptions options);
	}
}
