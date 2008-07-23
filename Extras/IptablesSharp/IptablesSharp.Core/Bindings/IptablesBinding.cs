// IptablesBinding.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez [mangelp]at[gmail]dot[com]
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
using System.IO;
using System.Text;

using Developer.Common.Unix.SystemCommands;

namespace IptablesSharp.Core.Bindings
{
	/// <summary>
	/// Generic binding to use iptables
	/// </summary>
	/// <remarks>
	/// This binding encapsulates the access to iptables functionality using calls to the
	/// iptables command or other libraries (if someday a stable api library is released).
	/// This class instances a concrete binding and then all the work is done by the
	/// concrete binding.
	/// </remarks>
	public class IptablesBinding
	{
		public IptablesBinding()
		{
		}
	}
}
