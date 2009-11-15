// TextOutputCommand.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by Miguel Angel Perez, mangelp<at>gmail(dot)com
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
using System.Diagnostics;
using System.Collections.Generic;

using CommonUtilities.SystemCommands;

namespace CommonUtilities.SystemCommands
{
	/// <summary>
	/// Models a system command that is expected to output information after
	/// executing it.
	/// </summary>
	public abstract class TextOutputCommand: BaseSystemCommand
	{
		/// <summary>
		/// Gets if the command can exec synchronously
		/// </summary>
		public override bool CanExec
		{
			get { return true; }
		}
		
		/// <summary>
		/// Gets if the command can exec asynchronously
		/// </summary>
		public override bool CanExecAsync
		{
			get { return true; }
		}
		
		/// <summary>
		/// Gets if the command can read from process stdout
		/// </summary>
		public override bool CanRead
		{
			get { return true;}
		}
		
		/// <summary>
		/// Gets if the command can read from process stdout
		/// </summary>
		public override bool CanReadError
		{
			get { return true;}
		}
		
		/// <summary>
		/// Gets if the command can write to process stdin
		/// </summary>
		public override bool CanWrite
		{
			get {return false;}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="commandName">Name with or without full path of the command to execute</param>
		/// <remarks>
		/// If the commandName is not a full path the command must be in the search path or this will
		/// not work.
		/// </remarks>
		public TextOutputCommand(string commandName)
			:base(commandName)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cmdName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="requiresRoot">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public TextOutputCommand(string cmdName, bool requiresRoot)
			:base(cmdName, requiresRoot)
		{
			
		}
		
		private string result;
		private bool ownRead = false;
		
		public string Read()
		{
			if (this.IsRunning)
				throw new InvalidOperationException("The process must not be started when using this method");
			
			this.ownRead = true;
			this.SyncReadMode = ReadMode.All;	
			result = String.Empty;
			this.Exec();
			this.ownRead = false;
			return result;
		}
		
		protected override void OnOutputReceivedHandler (string data)
		{
			if (this.ownRead)
				this.result = data;
			else
				base.OnOutputReceivedHandler(data);
		}

	}
}
