// IpAddressRange.cs
//
//  Copyright (C) 2008 Miguel Angel Perez (mye://mangelp/at/gmail?dot=com)
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

namespace Developer.Common.Net
{
	
	/// <summary>
	/// Models a range of ip addresses.
	/// </summary>
	public class IPAddressRange
	{
		private IPAddress address;
		
		public IPAddress Address {
			get {
				return address;
			}
			set {
				address = value;
			}
		}
		
		private IPAddressMask mask;

		public IPAddressMask Mask {
			get {
				return mask;
			}
			set {
				mask = value;
			}
		}
		
		public IPAddressRange()
		{}
		
		public bool IsInRange(IPAddress addr)
		{
			throw new NotImplementedException();
		}
		
		public static IPAddressRange CreateRange(IPAddress start, IPAddress end)
		{
			throw new NotImplementedException();
		}
	}
}
