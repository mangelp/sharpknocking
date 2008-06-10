// ReadOnlyTargetExtension.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
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
using System.Collections;
using System.Collections.Generic;

using IptablesSharp.Core;

namespace IptablesSharp.Core.Extensions.ExtendedTarget
{
	/// <summary>
	/// ReadOnly list adapter for lists of targetExtensionHandler type.
	/// </summary>
	public class ReadOnlyTargetExtensionListAdapter: ReadOnlyListAdapter<TargetExtensionHandler>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="adapted">
		/// A <see cref="List`1"/>
		/// </param>
		public ReadOnlyTargetExtensionListAdapter(List<TargetExtensionHandler> adapted)
			:base(adapted)
		{}
	}
}
