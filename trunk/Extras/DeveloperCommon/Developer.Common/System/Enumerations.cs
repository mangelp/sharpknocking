// Enumerations.cs
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
	/// Describes the set of known *nix clones
	/// </summary>
	public enum UnixPlatform
	{
		/// <summary>
		/// Other
		/// </summary>
		Other=0,
		/// <summary>
		/// Debian distro
		/// </summary>
		Debian=1,
		/// <summary>
		/// Fedora
		/// </summary>
		Fedora=2,
		/// <summary>
		/// Mandriva
		/// </summary>
		Mandriva=3,
		/// <summary>
		/// OpenBsd
		/// </summary>
		OpenBsd=4,
		/// <summary>
		/// OpenSolaris
		/// </summary>
		OpenSolaris=5,
		/// <summary>
		/// Suse
		/// </summary>
		Suse=6,
		/// <summary>
		/// Ubuntu
		/// </summary>
		Ubuntu=7
	}
}
