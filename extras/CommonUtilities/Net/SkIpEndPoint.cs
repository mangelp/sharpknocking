// SKIpEndPoint.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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


using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace CommonUtilities.Net
{
	/// <summary>
	/// End point for IP addresses.
	/// </summary>
	/// <remarks>
	/// This is a generic end point which supports ipv4 and ipv6 addresses setting it
	/// in the constructor.
	/// </remarks>
	public class SkIpEndPoint: EndPoint
	{
		/// <summary>
		/// Gets if the end point is ipV6 
		/// </summary>
		public bool isIpV6
		{
			get {return this.addressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;}
		}
		
		private byte[] address;
		
		/// <summary>
		/// Gets the address as a set of bytes 
		/// </summary>
		public byte[] Address{
			get { return this.address;}
		}
		
		private System.Net.Sockets.AddressFamily addressFamily;
		
		/// <summary>
		/// Gets the address family 
		/// </summary>
		public override System.Net.Sockets.AddressFamily AddressFamily {
			get { return this.addressFamily; }
		}
		
		private ProtocolType protocol;
		
		/// <summary>
		/// Type of protocol used by the endpoint.
		/// </summary>
		public ProtocolType Protocol
		{
			get {return this.protocol;}
			set {this.protocol = value;}
		}
		
		private ushort port;
		
		/// <summary>
		/// Port number used by this endpoint.
		/// </summary>
		public ushort Port
		{
			get {return this.port;}
			set {this.port = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// By default the type is ipv4. This behaviour should change in a future
		/// when ipv4 becomes an historic option.
		/// </remarks>
		public SkIpEndPoint()
			:this(false)
		{
			
		}
		
		/// <summary>
		/// Builds a new instance from the specified address information
		/// </summary>
		/// <param name="sa">
		/// A <see cref="SocketAddress"/>
		/// </param>
		public SkIpEndPoint(SocketAddress sa)
		{
			if (sa.Family != System.Net.Sockets.AddressFamily.InterNetwork &&
			   sa.Family != System.Net.Sockets.AddressFamily.InterNetworkV6) {
				throw new ArgumentException("The address family is not allowed for this endpoint","sa");
			} else if (sa.Family == System.Net.Sockets.AddressFamily.InterNetwork && 
			         sa.Size!=4) {
				throw new ArgumentException("The number of bytes are incorrect for this endpoint","sa");
			} else if (sa.Family == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
			         sa.Size!=16) {
				throw new ArgumentException("The number of bytes are incorrect for this endpoint","sa");
			}
			
			this.addressFamily = sa.Family;
			this.address = new byte[sa.Size];
			//Copy the bytes of the address
			for(int i=0;i<sa.Size;i++)
				this.address[i]=sa[i];
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// </remarks>
		public SkIpEndPoint(bool isIpv6)
			:base()
		{
			if(isIpv6)
				this.addressFamily = System.Net.Sockets.AddressFamily.InterNetworkV6;
			else
				this.addressFamily = System.Net.Sockets.AddressFamily.InterNetwork;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// Inits the instance as an ipv4 address
		/// </remarks>
		public SkIpEndPoint(params byte[] addr)
		{
			if(addr.Length==4)
				this.addressFamily = System.Net.Sockets.AddressFamily.InterNetwork;
			else if(addr.Length==16)
				this.addressFamily = AddressFamily.InterNetworkV6;
			else
				throw new ArgumentException("Invalid number of arguments. Correct values are 4 or 16", "addr");
			
			this.address = new byte[addr.Length];
			addr.CopyTo(this.address,0);
		}
		
		/// <summary>
		/// Creates a new endpoint
		/// </summary>
		/// <param name="address">
		/// A <see cref="SocketAddress"/> to init the new endpoint with
		/// </param>
		/// <returns>
		/// A <see cref="EndPoint"/> with the desired address
		/// </returns>
		public override EndPoint Create (SocketAddress address)
		{
			return (EndPoint)new SkIpEndPoint(address);
		}
		
		/// <summary>
		/// Serializes the current address as a SocketAddress
		/// </summary>
		/// <returns>
		/// A <see cref="SocketAddress"/> with the address of the current endpoint
		/// </returns>
		public override SocketAddress Serialize ()
		{
			SocketAddress sa = new SocketAddress(this.addressFamily, this.address.Length);
			for(int i=0;i<this.address.Length;i++)
				sa[i] = this.address[i];
			return sa;
		}

		/// <summary>
		/// Returns an string representation of the current address
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current address of
		/// the endpoint
		/// </returns>
		public override string ToString ()
		{
			if(this.isIpV6)
			{
				//TODO: Fix this
				StringBuilder sb = new StringBuilder(address[0].ToString(),63);
				for(int i=1;i<address.Length;i++)
				{
					sb.Append(':');
					sb.Append(address[i]);
				}
				return sb.ToString();
			}
			else
			{
				StringBuilder sb = new StringBuilder(address[0].ToString(),15);
				for(int i=1;i<address.Length;i++)
				{
					sb.Append('.');
					sb.Append(address[i]);
				}
				return sb.ToString();
			}
		}
	}
}
