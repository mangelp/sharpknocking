// Structs.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by Miguel Angel Perez mangelp@gmail.com
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

namespace Developer.Common.Unix.Net
{
	/// <summary>
	/// Represents a network service like those in /etc/services
	/// </summary>
	public struct NetworkService
	{
		public static readonly NetworkService Empty = new NetworkService();
		
		public string Name;
		public Protocols Protocol;
		public ushort Port;
		
		public override string ToString ()
		{
			return Name+"@"+((ushort)Protocol)+":"+Port.ToString();
		}
		
		public NetworkService(string name, Protocols proto, ushort port)
		{
			this.Name = name;
			this.Protocol = proto;
			this.Port = port;
		}
		
		public static NetworkService Parse(string netServ)
		{
			int pos = netServ.IndexOf('@');
			if(pos>0 && pos<(netServ.Length-1))
			{
				int pos2 = netServ.IndexOf(':', pos+1);
				if(pos2>(pos+1) && pos2<(netServ.Length-1))
				{
					NetworkService nsResult = new NetworkService();
					nsResult.Protocol = (Protocols) ushort.Parse(netServ.Substring(pos+1, pos2 -pos));
					nsResult.Port = ushort.Parse(netServ.Substring(pos2+1));
					nsResult.Name = netServ.Substring(0, pos);
					return nsResult;
				}
			}

			throw new FormatException("Invalid format for input string");
		}
		
		public override int GetHashCode ()
		{
			return this.ToString().GetHashCode();
		}
	}
}
