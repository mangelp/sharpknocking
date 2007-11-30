// TextOutputCommand.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by Miguel Angel Perez, mangelp@gmail.com
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
using System.Collections.Generic;

using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	/// <summary>
	/// Models a system command that is expected to output information after
	/// executing it.
	/// </summary>
	public class TextOutputCommand: BaseCommandWrapper
	{
		public override bool CanExec
		{
			get { return true; }
		}
		
		public override bool CanExecAsync
		{
			get { return true; }
		}
		
		public override bool CanRead
		{
			get { return true;}
		}
		
		public override bool CanWrite
		{
			get {return false;}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="commandName>Name with or without full path of the command to execute</param>
		/// <remarks>
		/// If the commandName is not a full path the command must be in the search path or this will
		/// not work.
		/// </remarks>
		public TextOutputCommand(string commandName)
			:base(commandName)
		{
		}
		
		public TextOutputCommand(string cmdName, bool requiresRoot)
			:base(cmdName, requiresRoot)
		{
			
		}
		
		protected override Process GetNewProcess ()
		{
			Process p = base.GetNewProcess ();
			p.StartInfo.RedirectStandardOutput = true;
			return p;
		}
		
		public override void Stop ()
		{
			if(this.IsAsync && this.Current!=null && !this.Current.HasExited)
			{
				IsAsync = false;
				this.Current.CancelOutputRead();
				this.Current.Kill();
				this.OnAsyncReadEnd(true);
			}
		}
		
		public override CommandResult Exec ()
		{
			this.Result.Clear();
			this.Current = this.GetNewProcess();
			
			try	{
				Current.Start();
				
				string str = Current.StandardOutput.ReadLine();
				//Read lines until null is returned.
				while(str!=null)
				{
					Result.Add(str);
					str = Current.StandardOutput.ReadLine();
				}
			} catch(Exception ex) {
				Console.Out.WriteLine(ex);
			}
			
			try {
				//Before requesting the exit code we must await the process to exit
				Current.WaitForExit();
				this.ExitCode = Current.ExitCode;
				Current.Close();
			} catch (Exception ex) {
				Console.Out.WriteLine(ex);
			}
			finally{
				Current = null;
			}
			
			CommandResult cres = new CommandResult();
			cres.Aborted = false;
			cres.Detail = String.Empty;
			cres.ExitCode = this.ExitCode;
			cres.UserData = Result.ToArray();
			this.Result.Clear();
			
			return cres;
		}
		
		public override void ExecAsync ()
		{
			this.Result.Clear();
			Current= this.GetNewProcess();
			Current.OutputDataReceived+= new DataReceivedEventHandler(this.OnDataReceivedHandler);
			Current.Exited += new EventHandler(this.OnAsyncReadEndHandler);
			IsAsync = true;
			this.ExitCode = -1;
			
			try {
				Current.BeginOutputReadLine();
				Current.Start();
			} catch (Exception ex) {
				Console.Out.WriteLine(ex);
				Current.Close();
				Current = null;
				IsAsync = false;
			}
		}
		
		public override void Write (string data)
		{
			throw new NotSupportedException("Write is not supported");
		}


	}
}
