// SKIpEndPoint.cs
//
//  Copyright (C) SharpKnocking Project 2007
//  Author: ${author}
//  For a list of contributors see AUTHORS
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

namespace Developer.Common
{
	/// <summary>
	/// End point for IP addresses.
	/// </summary>
	/// <remarks>
	/// This is a generic end point which supports ipv4 and ipv6 addresses setting it
	/// in the constructor.
	/// </remarks>
	public class SKIpEndPoint: EndPoint
	{
		public bool isIpV6
		{
			get {return this.address == AddressFamily.InterNetwork6;}
		}
		
		private byte[] address;
		
		public byte[] Address{
			get { return this.address;}
		}
		
		
		private AddressFamily addrFamily;
		
		public override AddressFamily AddressFamily {
			get { return this.addrFamily; }
		}
		
		public SKIpEndPoint()
			:this(false)
		{
			
		}
		
		/// <summary>Constructor</summary>
		/// <remarks></remarks>
		public SKIpEndPoint(bool is4Nor6)
			:base()
		{
			if(is4Nor6)
				addrFamily = AddressFamily.InterNetwork;
			else
				addrFamily = AddressFamily.InterNetwork6;
		}
		
		public override EndPoint Create (SocketAddress address)
		{
			SkIpEndPoint ep = new SkIpEndPoint(address
		}


	}
}
