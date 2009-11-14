// Knocks.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{aT}gmail[D0T]com)
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
using System.Net;

using Developer.Common.Net;

namespace iSharpKnocking.Core
{
	public class GenericKnock
	{}
	
	/// <summary>
	///  
	/// </summary>
	public class IpKnock: GenericKnock
	{
		public string Src;
		public string Dst;
		public string Id;
		public byte[] Data;
		public string Hash
		{
			get { return String.Empty; }
		}
	}
	
	public class TcpKnock: IpKnock
	{
		public int SPort;
		public int DPort;
		public int SeqNum;
		public string Hash
		{
			get { return base.Hash+SPort+DPort; }
		}
	}
	
	public class UdpKnock: IpKnock
	{
		public int SPort;
		public int DPort;
		public string Hash
		{
			get { return base.Hash+SPort+DPort; }
		}
	}
	
	public class IcmpKnock: IpKnock
	{
		public IcmpTypes Type;
		public string Hash
		{
			get { return base.Hash+((int)Type); }
		}
	}
	
	public struct SkKnock
	{
		public int Order;
		public string Name;
		public int Id;
	}
}
