// BaseCommandWrapper.cs
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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Developer.Common.SystemCommands
{
	public abstract class BaseCommandWrapper
	{
		private string name;
		
		/// <summary>
		/// Name of the command
		/// </summary>
		public string Name
		{
			get {return this.name;}
		}
		
		public string path;
		
		/// <summary>
		/// Command path if it has got one
		/// </summary>
		public string Path
		{
			get {return this.path;}
			set {this.path = value;}
		}
		
		private string args;
		
		/// <summary>
		/// List of arguments for the command
		/// </summary>
		public string Args
		{
			get {return this.args;}
			set {this.args = value;}
		}
		
		private StringDictionary enviroment;
		
		/// <summary>
		/// Dictionary of enviroment variables and values for the command
		/// </summary>
		/// <remarks>
		/// If it is empty the current enviroment is passed to the command.
		/// </remarks>
		public StringDictionary Enviroment
		{
			get { return this.enviroment;}
			set { this.enviroment = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// As it is an abstract class you must call this from your concrete
		/// non-abstract class.
		/// </remarks>
		public BaseCommandWrapper(string name)
		{
			enviroment = new StringDictionary();
			this.name = name;
		}

		/// <summary>
		/// Inheritors must define this method to execute the command
		/// </summary>
		/// <remarks>
		/// The current process will be bloqued until this methods ends its
		/// execution.
		/// </remarks>
		/// <returns>
		/// An object with the result of the command. See each command documentation
		/// for the real return format.
		/// </returns>
		public abstract CommandResult Exec ();
		
		/// <summary>
		/// Inheritors must define this method to execute the command asynchronously
		/// </summary>
		/// <remarks></remarks>
		public abstract void ExecAsync();
		
		/// <summary>
		/// Aborts the executing process. Only works if the execution was started
		/// with ExecAsync
		/// </summary>
		public abstract void Abort();
		
		/// <summary>
		/// End of command execution notifier
		/// </summary>
		public event EventHandler<CommandEndEventArgs> CommandEnd;
		
		protected void OnCommandEnd(CommandEndEventArgs args)
		{
			if(this.CommandEnd!=null)
				this.CommandEnd(this, args);
		}
		
		protected Process GetNewProcess()
		{
			Process p = new Process();
			p.StartInfo.FileName = this.name;
			p.StartInfo.Arguments = this.args;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			
			if(this.enviroment.Count>0)
			{
				foreach(string key in this.enviroment.Keys)
				{
					if(p.StartInfo.EnvironmentVariables.ContainsKey(key))
						p.StartInfo.EnvironmentVariables[key] = this.enviroment[key];
					else
						p.StartInfo.EnvironmentVariables.Add(key, this.enviroment[key]);
				}
			}
			return p;
		}
	}
}
