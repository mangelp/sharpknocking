// Definitions.cs
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

namespace Developer.Common.SystemCommands
{
	/// <summary>
	/// Data to identify the underliying os and the platform over it runs.
	/// </summary>
	public struct OsInfo
	{
		/// <summary>
		/// System OS version
		/// </summary>
		/// <remarks>
		/// This should be for examble the version of the kernel. IE 2.6.23
		/// </remarks>
		public Version OsVersion;
		
		/// <summary>
		/// System OS kind
		/// </summary>
		/// <remarks>
		/// This should be for examble the type of system. IE: Unix, Win32NT, ...
		/// </remarks>
		public PlatformID OsPlatform;
		
		/// <summary>
		/// Version of the mono runtime
		/// </summary>
		public Version MonoVersion;
		
		/// <summary>
		/// Distribution version
		/// </summary>
		/// <remarks>
		/// Version for the distribution in case of oses that comes in different
		/// flavours called "distros". 
		/// </remarks>
		public string DistroVersion;
		
		/// <summary>
		/// Distribution name
		/// </summary>
		/// <remarks>
		/// Name for the distribution in case of oses that comes in different
		/// flavours called "distros". 
		/// </remarks>
		public string DistroName;
	}
	
	/// <summary>
	/// Models the results of a command
	/// </summary>
	public struct CommandResult
	{
		/// <summary>
		/// Exit code of the command
		/// </summary>
		public int ExitCode;
		/// <summary>
		/// User data related to the finalization of the command
		/// </summary>
		public object UserData;
		/// <summary>
		/// Descriptive string with the results of the command
		/// </summary>
		public string Detail;
		/// <summary>
		/// Gets if the command didn't ended as it was killed or aborted
		/// </summary>
		public bool Aborted;
	}
}
