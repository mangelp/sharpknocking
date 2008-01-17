// Log.cs
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

namespace Developer.Common.Debuging
{	
	/// <summary>
	/// Application logging facility
	/// </summary>
	public static class AppLog
	{   
		/// <summary>
		/// Adds a message to the log output
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="ex">
		/// A <see cref="Exception"/>
		/// </param>
		public static void Add(string msg, Exception ex)
		{
			throw new NotImplementedException("Not Implemented");
		}
		
		/// <summary>
		/// Adds a message to the log output
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="origin">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="ex">
		/// A <see cref="Exception"/>
		/// </param>
		public static void Add(string msg, string origin, Exception ex)
		{
			throw new NotImplementedException("Not Implemented");		
		}
	}
}
