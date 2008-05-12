// Definitions.cs
//
//  Copyright (C) 2008 [name of author]
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

namespace Developer.Common.System
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
		/// This should be for example the version of the kernel. IE 2.6.23
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
		
		/// <summary>
		/// Gets an struct with the information about the platform, framework and distro.
		/// </summary>
		/// <returns>
		/// A <see cref="OsInfo"/>
		/// </returns>
		public static OsInfo GetInfo()
		{
			OsInfo osInfo = new OsInfo();
			osInfo.MonoVersion = (Version)Environment.Version.Clone();
			osInfo.OsPlatform = Environment.OSVersion.Platform;
			osInfo.OsVersion = (Version)Environment.OSVersion.Version.Clone();
			if(osInfo.OsPlatform == PlatformID.Unix) {
				osInfo.DistroVersion = UnixOsDetect.Version;
				osInfo.DistroName = UnixOsDetect.Platform.ToString();
			} else {
				osInfo.DistroVersion = Environment.OSVersion.ServicePack.ToString();
				osInfo.DistroName = "Windows";
			}
			
			return osInfo;
		}
	}
	
}