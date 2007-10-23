// ISysCommandWrapper.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by mangelp@gmail.com
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Developer.Common.SystemCommands
{
	
	/// <summary>
	/// Information required to handle commands built-in the shell or as
	/// external executables
	/// </summary>
	public interface ISysCommandWrapper
	{
		/// <summary>
		/// Name of the command 
		/// </summary>
		string Name {get;set; }
		
		/// <summary>
		/// Command path if it has got one
		/// </summary>
		string Path {get;set;}
		
		/// <summary>
		/// List of arguments for the command
		/// </summary>
		List<object> Args{get;}
		
		/// <summary>
		/// Dictionary with all the enviroment vars required for the command.
		/// </summary>
		StringDictionary Enviroment{get;}
		
		/// <summary>
		/// Determines if the command is going to be executed in a separate thread 
		/// </summary>
		bool UseThread{get;set;}
		
		/// <summary>
		/// Executes the command with no parameters
		/// </summary>
		void Exec();
		
		/// <summary>
		/// Executes the command with no parameters asynchronously
		/// </summary>
		void ExecAsync();
		
		event EventHandler<CommandEndEventArgs> CommandEnd;
	}
}
