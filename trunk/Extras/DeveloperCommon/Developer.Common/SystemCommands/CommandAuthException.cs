// CommandAuthException.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez <mangelp>at<gmail>dot<com>
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

namespace Developer.Common
{
	/// <summary>
	/// Error with the authorization to start a command that requires it
	/// </summary>
	public class CommandAuthException: Exception
	{
		/// <summary>
		/// Constructor. Initializes the message to the default one.
		/// </summary>
		public CommandAuthException()
			:base("Authentication for the underliying command failed")
		{
		}
		
		/// <summary>
		/// Constructor that initiallizes the message
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		public CommandAuthException(string msg)
			:base(msg)
		{}
		
		/// <summary>
		/// Constructor that initiallizes the message and the inner exception
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="innerEx">
		/// A <see cref="Exception"/>
		/// </param>
		public CommandAuthException(string msg, Exception innerEx)
			:base(msg, innerEx)
		{}
	}
}
