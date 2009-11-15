// OptionList.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez <mangelp>at<gmail>dot<com>
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
using System.Collections.Generic;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat
{
	/// <summary>
	/// List of options. Each element must extend Option class
	/// </summary>
	public class OptionList<T>: List<T> where T:Option
	{
		/// <summary>
		/// Returns the first option with an alias or null if no option has that
		/// alias.
		/// </summary>
		public T this[string alias]
		{
			get {
				int pos = this.IndexOf(alias);
				if (pos == -1)
					return null;
				return this[pos];
			}
		}
		
		public OptionList()
			:base()
		{
		}
		
		/// <summary>
		/// Returns the position of the first option with an alias
		/// </summary>
		/// <param name="alias">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public int IndexOf(string alias)
		{
			for(int i=0; i<this.Count; i++) {
				if (this[i].IsAlias(alias))
					return i;
			}
			
			return -1;
		}
		
		/// <summary>
		/// Gets if there is an option with that alias
		/// </summary>
		/// <param name="alias">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool HasOptionWithAlias(string alias)
		{
			return this.IndexOf(alias) >= 0;
		}
	}
}
