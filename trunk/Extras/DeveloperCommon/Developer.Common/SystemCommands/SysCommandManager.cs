// SysCommandManager.cs
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
using System.Collections.Generic;

namespace Developer.Common.SystemCommands
{
	
	/// <summary>
	/// Defines methods to handle execution of commands in the system and
	/// to retrieve some configuration data required
	/// </summary>
	public class SysCommandManager
	{
		private static SysCommandManager instance;
		
		/// <summary>
		/// Singleton instance of the command manager.
		/// </summary>
		public static SysCommandManager Instance
		{
			get {
				if(instance==null)
					instance = new SysCommandManager();
				return instance;
			}
		}
		
		private Dictionary<string, BaseCommandWrapper> commands;
		
		private OsInfo osInfo;
		
		/// <summary>
		/// Gets the information about the current executing platform
		/// </summary>
		/// <returns>
		/// A <see cref="Developer.Common.SystemCommands"/> with all the
		/// information
		/// </returns>
		public OsInfo CurrentOsInfo
		{
			get {
				return osInfo;
			}
		}
		
		private SysCommandManager()
		{
			commands = new Dictionary<string, BaseCommandWrapper>();
			osInfo = new OsInfo();
			osInfo.MonoVersion = (Version)Environment.Version.Clone();
			osInfo.OsPlatform = Environment.OSVersion.Platform;
			osInfo.OsVersion = (Version)Environment.OSVersion.Version.Clone();
			if(osInfo.OsPlatform == PlatformID.Unix)
			{
				;
			}
			else
			{
				osInfo.DistroVersion = Environment.OSVersion.ServicePack.ToString();
				osInfo.DistroName = "Windows";
			}
		}
		
		/// <summary>
		/// Adds a command to be managed
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="cmd">
		/// A <see cref="BaseCommandWrapper"/>
		/// </param>
		public void AddCommand(string name, BaseCommandWrapper cmd)
		{
			commands.Add(name, cmd);
		}
		
		/// <summary>
		/// Determines if a certain command is running
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the command
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> that indicates if the command is
		/// running or not.
		/// </returns>
		public bool IsCommandRunning(string name)
		{
			return this.commands.ContainsKey(name) && this.commands[name].IsRunning;
		}
		
		/// <summary>
		/// Kills a command 
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the command to kill
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> that indicates if the command was
		/// successfully killed.
		/// </returns>
		public void KillCommand(string name)
		{
			if(this.commands.ContainsKey(name)) {
				this.commands[name].Stop();
				this.commands.Remove(name);
			}
		}
	}
}
