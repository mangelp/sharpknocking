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
		private Process current;
		private bool isAsync;
		private List<string> result;
		private int exitCode;
		
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
		
		public override void Abort ()
		{
			if(this.isAsync && !this.current.HasExited)
			{
				isAsync = false;
				this.current.CancelOutputRead();
				this.current.Kill();
				this.OnAsyncReadEnd(true);
			}
		}
		
		public override CommandResult Exec ()
		{
			result = new List<string>();
			current = this.GetNewProcess();
			
			try	{
				current.Start();
				
				string str = current.StandardOutput.ReadLine();
				//Read lines until null is returned.
				while(str!=null)
				{
					result.Add(str);
					str = current.StandardOutput.ReadLine();
				}
			} catch(Exception ex) {
				Console.Out.WriteLine(ex);
			}
			
			try {
				//Before requesting the exit code we must await the process to exit
				current.WaitForExit();
				this.exitCode = current.ExitCode;
				current.Close();
				current = null;
			} catch (Exception ex) {
				Console.Out.WriteLine(ex);
			}
			
			CommandResult cres = new CommandResult();
			cres.Aborted = false;
			cres.Detail = String.Empty;
			cres.ExitCode = this.exitCode;
			cres.UserData = result;
			
			return cres;
		}
		
		public override void ExecAsync ()
		{
			result = new List<string>();
			current= this.GetNewProcess();
			current.OutputDataReceived+= new DataReceivedEventHandler(this.OnDataReceivedHandler);
			current.Exited += new EventHandler(this.OnAsyncReadEndHandler);
			
			try {
				isAsync = true;
				current.BeginOutputReadLine();
				current.Start();
			} catch (Exception ex) {
				Console.Out.WriteLine(ex);
			} finally {
				this.exitCode = current.ExitCode;
				current.Close();
				current = null;
			}
		}
		
		private void OnDataReceivedHandler(object sender, DataReceivedEventArgs args)
		{
			result.Add(args.Data);
		}
		
		private void OnAsyncReadEndHandler(object sender, EventArgs args)
		{
			this.OnAsyncReadEnd(false);
		}
		
		private void OnAsyncReadEnd(bool aborted)
		{
			this.current.Close();
			this.current = null;
			
			CommandResult res = new CommandResult();
			res.UserData = result;
			res.Aborted = aborted;
			res.ExitCode = this.exitCode;
			CommandEndEventArgs args = new CommandEndEventArgs(res);
			
			this.OnCommandEnd(args);
		}
	}
}
