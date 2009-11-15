// BaseCommandWrapper.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp(at)gmail[dot]com
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
using System.Timers;
using System.Security;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CommonUtilities.SystemCommands
{
	/// <summary>
	/// Base class for executing command-line application throught Process class
	/// </summary>
	public abstract class BaseSystemCommand
	{
		#region Events
		/// <summary>
		/// Notifies that data have been read from the standard output
		/// </summary>
		public event EventHandler<OutputReadEventArgs> OutputRead;
		/// <summary>
		/// Notifies that data have been read from the standard error
		/// </summary>
		public event EventHandler<OutputReadEventArgs> ErrorRead;
		/// <summary>
		/// End of command execution notifier
		/// </summary>
		public event EventHandler<CommandEndEventArgs> CommandEnd;
		/// <summary>
		/// Notifies that the command have been started
		/// </summary>
		public event EventHandler CommandStart;
		/// <summary>
		/// Notifies that authentication is required to run the command
		/// </summary>
		public event EventHandler<AuthRequiredEventArgs> AuthRequired;
		
		#endregion
		
		#region properties and fields
		
		private Timer killTimer;
		
		private Process current;
		
		/// <summary>
		/// Current Process instance that is being executed.
		/// </summary>
		/// <remarks>
		/// If there is no process running this reference can be null
		/// </remarks>
		protected Process Current
		{
			get { return this.current;}
		}
		
		private bool isAsync;
		
		/// <summary>
		/// Indicates if the command is executing or will be executing asynchronously
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		protected bool IsAsync
		{
			get { return this.isAsync;}
			set { 
				if(value && !this.CanExecAsync)
					throw new InvalidOperationException("command process can't execute asynchronously");
				if (!value && !this.CanExec)
					throw new InvalidOperationException("Command can't execute synchronously");
				this.isAsync = value;
			}
		}
		
		private int exitCode;
		
		/// <summary>
		/// Exit code of the process if it ended correctly
		/// </summary>
		public int ExitCode
		{
			get { return this.exitCode;}
			protected set { this.exitCode = value;}
		}
		
		private string cmdName;
		
		/// <summary>
		/// Name of the command
		/// </summary>
		public string CmdName
		{
			get {return this.cmdName;}
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
		
		/// <summary>
		/// Gets if the process can be executed synchronously
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		public abstract bool CanExec {get;}
		
		/// <summary>
		/// Gets if the process can be executed asynchronously
		/// </summary>
		public abstract bool CanExecAsync {get;}
		
		/// <summary>
		/// Gets if the standard output can be read
		/// </summary>
		public abstract bool CanRead {get;}
		
		/// <summary>
		/// Gets if the standard error output can be read
		/// </summary>
		public abstract bool CanReadError {get;}
		
		/// <summary>
		/// Gets if the standard input can be writen
		/// </summary>
		public abstract bool CanWrite {get;}
		
		private ReadMode syncReadMode;
		
		/// <summary>
		/// Mode for reads. This determines if the data is read line by line or
		/// it is read entirely and then returned.
		/// </summary>
		public ReadMode SyncReadMode {
			get { return this.syncReadMode;}
			set { this.syncReadMode = value;}
		}

		private bool isAuthRequired;
		
		/// <summary>
		/// Gets if authentication is required to run the current command
		/// </summary>
		protected bool IsAuthRequired
		{
			get { return this.isAuthRequired; }
		}
		
		private string executeAs;
		
		/// <summary>
		/// Gets/Sets the name of the local user to use to execute the system command
		/// </summary>
		public string ExecuteAs
		{
			get {return this.executeAs;}
			set {
				this.executeAs = value;
				this.isAuthRequired = !String.IsNullOrEmpty(value);
			}
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
		/// Gets if the underlying process is running or not
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		public bool IsRunning
		{
			get {
				return !(this.current==null || this.current.HasExited);
			}
		}
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// As it is an abstract class you must call this from your concrete
		/// non-abstract class.
		/// </remarks>
		public BaseSystemCommand(string name)
		{
			enviroment = new StringDictionary();
			this.cmdName = name;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the command to execute
		/// </param>
		/// <param name="authRequired">
		/// A <see cref="System.Boolean"/> that indicates if the command requires root privileges
		/// to work properly
		/// </param>
		public BaseSystemCommand(string name, bool authRequired)
			: this(name)
		{
			this.isAuthRequired = authRequired;
			if (authRequired)
				this.executeAs = this.GetAdminName();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cmdName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="username">
		/// A <see cref="System.String"/>
		/// </param>
		public BaseSystemCommand(string cmdName, string username)
			: this(cmdName)
		{
			this.ExecuteAs = username;
		}
		
		#endregion
		
		#region Notifiers and event handlers
		
		/// <summary>
		/// Waits for the process to finish and recovers the exit code, then notifies about the end
		/// of the command.
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="args">
		/// A <see cref="EventHandler"/>
		/// </param>
		protected virtual void OnProcessEnd(object sender, EventArgs args)
		{
			Console.WriteLine("Process end at "+DateTime.Now);
			CommandResult result = this.WaitForEnd();
			if (this.CommandEnd != null) {
				CommandEndEventArgs cmdargs = new CommandEndEventArgs(result);
				this.CommandEnd(this, cmdargs);
			}
		}
		
		/// <summary>
		/// Notifies that the command have been started
		/// </summary>
		protected virtual void OnCommandStart()
		{
			if (this.CommandStart != null) {
				this.CommandStart(this, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Event handler to notify clients about the output data that have been read
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/> which is the reference to the object who sent the event.
		/// </param>
		/// <param name="args">
		/// A DataReceivedEventArgs whith the data received from the process
		/// </param>
		protected virtual void OnOutputReceivedHandler(object sender, DataReceivedEventArgs args)
		{
			if (this.OutputRead != null) {
				OutputReadEventArgs readArgs = new OutputReadEventArgs(args.Data);
				this.OutputRead(this, readArgs);
			}
		}
		
		/// <summary>
		/// Helper to notify clients about the output data that have been read
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.String"/>
		/// </param>
		protected virtual void OnOutputReceivedHandler(string data)
		{
			if (this.OutputRead != null) {
				OutputReadEventArgs readArgs = new OutputReadEventArgs(data);
				this.OutputRead(this, readArgs);
			}
		}
		
		/// <summary>
		/// Handler for the DataReceived event from standard error
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/> which is the reference to the object who sent the event.
		/// </param>
		/// <param name="args">
		/// A DataReceivedEventArgs whith the data received from the process
		/// </param>
		protected virtual void OnOutputErrorReceivedHandler(object sender, DataReceivedEventArgs args)
		{
			if (this.ErrorRead != null) {
				OutputReadEventArgs readArgs = new OutputReadEventArgs(args.Data);
				this.ErrorRead(this, readArgs);
			}
		}
		
		/// <summary>
		/// Sends the notification about the required authentication.
		/// </summary>
		/// <remarks> Usually clients will want to use this or switch to custom os
		/// impersonation.
		/// </remarks>
		/// <param name="args">
		/// A <see cref="AuthRequiredEventArgs"/>
		/// </param>
		protected virtual void OnAuthRequired(AuthRequiredEventArgs args)
		{
			//If no auth is required return as if it where successfull
			if (!this.isAuthRequired) {
				args.Success = true;
				return;
			}
			
			//Take care of getting a user name
			if (String.IsNullOrEmpty(this.executeAs))
				args.UserName = this.GetAdminName();
			else
				args.UserName = this.executeAs;
			
			//If the delayed flag is on we omit notifications
			if (this.AuthRequired != null && !args.Delayed)
				this.AuthRequired(this, args);
		}
		
		#endregion
		
		#region protected stuff
		/// <summary>
		/// Returns the name of the administrator user for the current os
		/// </summary>
		/// <remarks>
		/// This usually is 'root' in unix and 'administrator' in windows.
		/// </remarks>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		protected abstract string GetAdminName();
		
		/// <summary>
		/// Performs authentication
		/// </summary>
		/// <remarks>
		/// This method calls OnAuthRequired to notify others and see if there are someone that
		/// will resolve authorization request providing either a password for the current user
		/// name or giving the auth for the process
		/// </remarks>
		/// <returns>
		/// A <see cref="System.Boolean"/> true if the auth is successfull or if it is delayed
		/// </returns>
		protected bool DoAuth()
		{
			if (!this.isAuthRequired)
				return true;
			
			AuthRequiredEventArgs args = new AuthRequiredEventArgs(this.executeAs);
			
			this.OnAuthRequired(args);
			
			if (args.Delayed && args.Password.Length > 0)
				this.current.StartInfo.Password = args.Password;
			
			if (args.Delayed && !String.IsNullOrEmpty(args.UserName))
				this.current.StartInfo.UserName = args.UserName;
				
			return args.Delayed || args.Success;
		}
		
		/// <summary>
		/// Initializes the execution of the process
		/// </summary>
		protected void Start()
		{
			if (this.current != null && this.current.HasExited)
				this.current.Close();
			else if (this.current != null && !this.current.HasExited)
				this.current.Kill();
			
			this.current = this.GetNewProcess();
			this.current.Exited += new	EventHandler(this.OnProcessEnd);
			this.exitCode = Int32.MaxValue;
			
			if (this.CanRead && this.isAsync) {
				this.current.OutputDataReceived += 
					new DataReceivedEventHandler(this.OnOutputReceivedHandler);
			}
			
			if (this.CanReadError) {
				this.current.ErrorDataReceived += 
					new DataReceivedEventHandler(this.OnOutputErrorReceivedHandler);
			}
			
			if (!this.DoAuth()) {
				throw new CommandAuthException();
			}
			
			this.current.Start();
			
			if (this.CanRead && this.isAsync)
				this.current.BeginOutputReadLine();
			
			if (this.CanReadError)
				this.current.BeginErrorReadLine();
			
			this.OnCommandStart();
		}
		
		/// <summary>
		/// Waits for the current process to finish and sets the exit code
		/// </summary>
		/// <remarks>
		/// This method does a waitForExit for 30 seconds, after that time
		/// if the process haven't finished it tries to kill it.
		/// Finally it tries to call Close() once the process has exited or
		/// have been killed.
		/// </remarks>
		protected CommandResult WaitForEnd()
		{
			CommandResult result = new CommandResult();
			result.Aborted = false;
			result.Detail = String.Empty;
			result.UserData = null;
			
			try {
				
				//Before requesting the exit code we must wait the process to exit
				//we will wait for 30 secs
				this.current.WaitForExit(30000);
				//If after 30 seconds the exit code haven't been got we send it a kill
				if (!this.current.HasExited) {
					this.current.Kill();
					result.Aborted = true;
				}
				this.exitCode = this.current.ExitCode;
				result.ExitCode = this.exitCode;
				//Free resources
				this.current.Close();
			} catch (Exception ex) {
				throw ex;
			}
			finally{
				if (this.current != null)
					this.current.Dispose();
				this.current = null;
			}
			
			return result;
		}
		
		/// <summary>
		/// Gets a new process object partially configured to be started.
		/// </summary>
		/// <remarks>
		/// Inheritors should override this method calling first the parent one to
		/// get the process instance and then configure it properly.
		/// </remarks>
		/// <returns>
		/// A <see cref="Process"/> with the required command name
		/// </returns>
		protected virtual Process GetNewProcess()
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName = this.cmdName;
			p.StartInfo.Arguments = this.args;
			
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
			
			if (this.CanRead)
				p.StartInfo.RedirectStandardOutput = true;
			if (this.CanReadError)
				p.StartInfo.RedirectStandardError = true;
			if (this.CanWrite)
				p.StartInfo.RedirectStandardInput = true;
			
			return p;
		}
		
		/// <summary>
		/// Ends the executing process killing it.
		/// </summary>
		/// <remarks>
		/// This method is intended to stop the running process by any means and once it is stopped 
		/// clients will get notified throught
		/// </remarks>
		public virtual void Stop()
		{
			if (this.current == null)
				return;
			
			if (this.CanRead && this.isAsync)
				this.current.CancelOutputRead();
			
			if (this.CanReadError)
				this.current.CancelErrorRead();
			
			this.OnProcessEnd(this, EventArgs.Empty);
		}
		
		/// <summary>
		/// Kills the executing command
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="args">
		/// A <see cref="EventArgs"/>
		/// </param>
		protected virtual void KillHandler(object sender, EventArgs args)
		{
			Console.WriteLine("Timer kill for process " + this.cmdName);
			this.KillTimeoutEnd();

			if (this.current != null && !this.current.HasExited) {
				Console.WriteLine("Killing "+this.cmdName);
				this.current.Kill();
			} else {
				Console.WriteLine("Can't kill");
			}
		}
		
		/// <summary>
		/// Starts the timeout for 10 seconds (default)
		/// </summary>
		protected void KillTimeoutStart()
		{
			this.KillTimeoutStart(10000);
		}
		
		/// <summary>
		/// Starts the timeout for an amount of time
		/// </summary>
		/// <param name="millis">
		/// A <see cref="System.Int32"/> Milliseconds to set as interval for the timer
		/// </param>
		protected void KillTimeoutStart(int millis)
		{
			this.KillTimeoutEnd();
			
			this.killTimer = new Timer();
			this.killTimer.AutoReset = false;
			this.killTimer.Interval = millis;
			this.killTimer.Elapsed += new ElapsedEventHandler(this.KillHandler);
			
			Console.WriteLine("starting kill timer: "+millis);
			this.killTimer.Start();
		}
		
		/// <summary>
		/// Ends the timeout
		/// </summary>
		protected void KillTimeoutEnd()
		{
			if (this.killTimer == null)
				return;
			
			Console.WriteLine("Ending kill timer");	
			if (this.killTimer.Enabled)
				this.killTimer.Stop();
			
			this.killTimer.Dispose();
			this.killTimer = null;
		}
		
		/// <summary>
		/// Gets if the kill timeout is active
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		protected bool IsKillTimeoutActive()
		{
			return this.killTimer != null && this.killTimer.Enabled;
		}
		
		#endregion
		
		#region public stuff
		
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
		public virtual CommandResult Exec ()
		{
			if (!this.CanExec)
				throw new InvalidOperationException("This command can't be executed synchronously");
			this.IsAsync = false;
			
			this.Start();
			string data = null;
			
			if (this.CanRead) {
				
				do {
					data = null;
					switch(this.syncReadMode) {
						case ReadMode.Line:
							data = this.current.StandardOutput.ReadLine();
							break;
						case ReadMode.All:
							data = this.current.StandardOutput.ReadToEnd();
							//after this we must not try to read again
							this.OnOutputReceivedHandler(data);
							data = null;
							break;
					}
					if (data != null)
						this.OnOutputReceivedHandler(data);
				} while(this.IsRunning && data != null);
			}
			return this.WaitForEnd();
		}
		
		/// <summary>
		/// Inheritors must define this method to execute the command asynchronously
		/// </summary>
		/// <remarks></remarks>
		public virtual void ExecAsync()
		{
			IsAsync = true;
			
			this.Start();
		}
		
		/// <summary>
		/// Writes the data to the standard input of the running command
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.String"/>
		/// </param>
		public virtual void Write(string data)
		{
			if (!this.CanWrite) {
				throw new InvalidOperationException("You can't write to the command standard input");
			}
			
			if (!this.IsRunning) {
				throw new InvalidOperationException("The command is not running");
			}
			
			this.current.StandardInput.Write(data);
		}
		
		#endregion
	}
}
