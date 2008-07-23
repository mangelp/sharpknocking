// WhichSysCmd.cs
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
	/// Wrapper for the which command used to find executables under the search
	/// path.
	/// </summary>
	/// <remarks>
	/// This class is based in the class SearchWrapper created by Luis Román 
	/// for this task
	/// </remarks>
	public class WhichSysCmd: TextOutputCommand
	{
		/// <summary>
		/// Constructor. Initiallizes the command name to which.
		/// </summary>
		public WhichSysCmd() 
			: base("which")
		{
		}
	}
}
