// OptionParserException.cs
//
//  Copyright (C)  2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp_AT_gmail_DOT_com)
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

namespace Developer.Common.Options
{
	
	/// <summary>
	/// Exception for problems within the option parser
	/// </summary>
	public class OptionParserException: Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public OptionParserException()
			:base("Problem found while parsing the command line")
		{}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/> message of the exception
		/// </param>
		public OptionParserException(string msg)
			:base(msg)
		{}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg">
		/// A <see cref="System.String"/> message of the exception
		/// </param>
		/// <param name="innerEx">
		/// A <see cref="Exception"/> inner exception
		/// </param>
		public OptionParserException(string msg, Exception innerEx)
			:base(msg, innerEx)
		{}
	}
}
