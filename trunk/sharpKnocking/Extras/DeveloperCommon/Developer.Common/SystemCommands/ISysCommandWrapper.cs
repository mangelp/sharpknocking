// ISysCommandWrapper.cs
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
		string Name {get;}
		
		/// <summary>
		/// Command path if it has got one
		/// </summary>
		string Path {get;set;}
		
		/// <summary>
		/// Dictionary with all the enviroment vars required for the command.
		/// </summary>
		StringDictionary Enviroment{get;}
		
		/// <summary>
		/// Returns if the command is still alive
		/// </summary>
		bool IsRunning {get;set;}
		
		/// <summary>
		/// Executes the command with no parameters
		/// </summary>
		CommandResult Exec();
		
		/// <summary>
		/// Executes the command with no parameters asynchronously
		/// </summary>
		void ExecAsync();
		
		/// <summary>
		/// Event handler for the CommandEnd event
		/// </summary>
		event EventHandler<CommandEndEventArgs> CommandEnd;
		
		/// <summary>
		/// Tries to end the command gracefully
		/// </summary>
		void Terminate();
		
		/// <summary>
		/// Ends the command
		/// </summary>
		void Kill();
	}
}
