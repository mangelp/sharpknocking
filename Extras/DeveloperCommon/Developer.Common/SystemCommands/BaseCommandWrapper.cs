// BaseCommandWrapper.cs
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
		private Process current;
		
		protected Process Current
		{
			get { return this.current;}
			set { this.current = value;}
		}
		
		private bool isAsync;
		
		protected bool IsAsync
		{
			get { return this.isAsync;}
			set { this.isAsync = value;}
		}
		
		private List<string> result;
		
		protected List<string> Result
		{
			get { return this.result;}
		}
		
		private int exitCode;
		
		protected int ExitCode
		{
			get { return this.exitCode;}
			set { this.exitCode = value;}
		}
		
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
		
		public abstract bool CanExec {get;}
		
		public abstract bool CanExecAsync {get;}
		
		/// <summary>
		/// Gets if the standard output can be read
		/// </summary>
		public abstract bool CanRead {get;}
		
		/// <summary>
		/// Gets if the standard input can be write
		/// </summary>
		public abstract bool CanWrite {get;}
		
		private bool requiresRoot;
		
		/// <summary>
		/// If true the command requires root permissions to work.
		/// </summary>
		/// <remarks>
		/// This usually means that the program must be executed as root or
		/// that an autentication process must be set.
		/// </remarks>
		public bool RequiresRoot
		{
			get {return this.requiresRoot;}
			set {this.requiresRoot = value;}
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
			protected set { this.enviroment = value;}
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
			this.result = new List<string>();
		}
		
		public BaseCommandWrapper(string name, bool requiresRoot)
			: this(name)
		{
			this.requiresRoot = requiresRoot;
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
		/// Writes into the the standard input of the process
		/// </summary>
		public abstract void Write(string data);
		
		/// <summary>
		/// End of command execution notifier
		/// </summary>
		public event EventHandler<CommandEndEventArgs> CommandEnd;
		
		protected void OnCommandEnd(CommandEndEventArgs args)
		{
			if(this.CommandEnd!=null)
				this.CommandEnd(this, args);
		}
		
		/// <summary>
		/// Gets a new process object partially configured to be started.
		/// </summary>
		/// <returns>
		/// A <see cref="Process"/> with the required command name
		/// </returns>
		protected virtual Process GetNewProcess()
		{
			Process p = new Process();
			p.StartInfo.FileName = this.name;
			p.StartInfo.Arguments = this.args;
			p.StartInfo.UseShellExecute = false;
			
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

		protected void OnDataReceivedHandler(object sender, DataReceivedEventArgs args)
		{
			//TODO: If the data is being read asynchronously this should output it in the
			//same way not waiting to have all to output it. We don't know how many data
			//can be sent
			result.Add(args.Data);
		}
		
		protected void OnAsyncReadEndHandler(object sender, EventArgs args)
		{
			this.OnAsyncReadEnd(false);
		}
		
		protected virtual void OnAsyncReadEnd(bool aborted)
		{
			this.current.Close();
			this.current.WaitForExit();
			this.exitCode = this.current.ExitCode;
			this.current = null;
			
			CommandResult res = new CommandResult();
			res.UserData = result;
			res.Aborted = aborted;
			res.ExitCode = this.exitCode;
			CommandEndEventArgs args = new CommandEndEventArgs(res);
			
			this.OnCommandEnd(args);
		}
		
		protected virtual void OnAsyncWriteEndHandler(object sender, EventArgs args)
		{
			//TODO: How async writes works?
		}
	}
}
