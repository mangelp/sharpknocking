//  
//  Copyright (C) 2010 SharpKnocking project
//  File created by mangelp
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using NFSharp.Iptables.Core;
using CommonUtilities.Types;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {


	public class TableParser
	{

		public TableParser () {
		}

		/// <summary>
        /// Parses a string and builds a instance of a NetfilterTable object
        /// </summary>
		public NetfilterTable Parse(string line)
		{
			PacketTableType tp = PacketTableType.Filter;
			if(!NetfilterTable.TryGetTableType(line, out tp)) {
				return null;
			}

			return new NetfilterTable(tp);
		}
	}
}
