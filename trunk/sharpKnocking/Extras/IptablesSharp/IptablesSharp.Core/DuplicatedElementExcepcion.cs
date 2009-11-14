// DuplicateElementException.cs
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

namespace IptablesSharp.Core
{
	/// <summary>
	/// Exception thrown when an element is added twice to a collection where all the elements can only appear
	/// once.
	/// </summary>
	public class DuplicatedElementException: Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DuplicatedElementException()
		    :base()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		public DuplicatedElementException(string msg)
		    :base(msg)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="innerException">
		/// A <see cref="Exception"/>
		/// </param>
		public DuplicatedElementException(string msg, Exception innerException)
		    :base(msg, innerException)
		{
		}
	}
}
