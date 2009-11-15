// IpAddressHelper.cs
//
//  Copyright (C) 2008 [name of author]
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

using NFSharp.Common.Types;

namespace NFSharp.Common.Net
{
	/// <summary>
	/// IPAddress class wrapper that adds some little extra functionality
	/// </summary>
	public class IPAddressWrapper
	{
		private IPAddress ipAddr;
		
		/// <summary>
		/// Ip address object
		/// </summary>
		public IPAddress IpAddress
		{
			get {return this.ipAddr;}
		}
		
		private IPAddressMask mask;
		
		/// <summary>
		/// Ip address mask object
		/// </summary>
		public IPAddressMask Mask
		{
			get {
				return this.mask;
			}
			set {
				this.mask = value;
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ipAddr">
		/// A <see cref="IPAddress"/>
		/// </param>
		public IPAddressWrapper(IPAddress ipAddr)
		{
			this.ipAddr = ipAddr;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ipAddr">
		/// A <see cref="IPAddress"/>
		/// </param>
		/// <param name="mask">
		/// A <see cref="IPAddressMask"/>
		/// </param>
		public IPAddressWrapper(IPAddress ipAddr, IPAddressMask mask)
		{
			this.ipAddr = ipAddr;
			this.mask = mask;
		}
		
		/// <summary>
		/// Parses an string into an ip address of type IPAddress and a mask of type
		/// IPAddressMask
		/// </summary>
		/// <remarks>
		/// If there is no mask a null value will be used.
		/// </remarks>
		/// <param name="ipaddr">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="IPAddressWrapper"/>
		/// </returns>
		public static IPAddressWrapper Parse(string ipaddr)
		{
			if (String.IsNullOrEmpty(ipaddr))
				throw new ArgumentException("Can't parse an empty string or null reference", "ipaddr");
			
			IPAddress result = null;
			
			if (IPAddress.TryParse (ipaddr, out result)) {
				return new IPAddressWrapper(result);
			}
			//The string is not an ip address if we get here so it must have a mask.
			//If not it has an invalid format
			IPAddressWrapper wrapper = null;
			IPAddressMask mask = null;
			int pos = ipaddr.LastIndexOf('/');
			
			if (pos > 0) {
				//Parse ip address first
				if (!IPAddress.TryParse(ipaddr.Substring(0, pos), out result))
					throw new ArgumentException("Invalid format for ip address");
				//Parse mask
				if (!IPAddressMask.TryParse(ipaddr.Substring(pos + 1), out mask))
					throw new ArgumentException("Invalid format for mask");
				wrapper = new IPAddressWrapper(result, mask);
			} else {
				throw new ArgumentException("The ip address is not valid", "ipaddr");
			}
			
			return wrapper;
		}
		
		/// <summary>
		/// This method does the same as the Parse method but this will not throw an 
		/// exception.
		/// </summary>
		/// <remarks>
		/// Use the out value only if this method returns true. Otherwise its value will
		/// be not safe.
		/// </remarks>
		/// <param name="ipaddr">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="wrapper">
		/// A <see cref="IPAddressWrapper"/> with the result.
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>. True if the value was successfully parsed or
		/// false if not.
		/// </returns>
		public static bool TryParse(string ipaddr, out IPAddressWrapper wrapper)
		{
			wrapper = null;
			try {
				wrapper = IPAddressWrapper.Parse(ipaddr);
				return true;
			} catch (Exception) {
				return false;
			}
		}
		
		/// <summary>
		/// Gets if a ip address is in the range expressed by the current ip address
		/// and the mask.
		/// </summary>
		/// <remarks>
		/// If there is no ip address set or no mask this method will return
		/// false always.
		/// </remarks>
		/// <param name="ipAddr">
		/// A <see cref="IPAddress"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool MatchMask(IPAddress ipAddr)
		{
			if (this.ipAddr == null || this.mask == null)
				return false;
			//TODO: apply the mask to the ip and see the result
			return true;
		}
		
		/// <summary>
		/// Greater than comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator > (IPAddressWrapper left, IPAddressWrapper right)
		{
			byte[] leftb = left.ipAddr.GetAddressBytes();
			byte[] rightb = right.ipAddr.GetAddressBytes();
			
			if (leftb.Length != rightb.Length)
				throw new ArgumentException("Addresses are not the same ip version");
			
			for(int i=0; i<leftb.Length; i++) {
				if (leftb[i] < rightb[i])
					return false;
				else if (leftb[i] > rightb[i])
					return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Equally comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator == (IPAddressWrapper left, IPAddressWrapper right)
		{
			return ArrayComparer.Same(left.ipAddr.GetAddressBytes(),
				right.ipAddr.GetAddressBytes());
		}
		
		/// <summary>
		/// Less than comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator < (IPAddressWrapper left, IPAddressWrapper right)
		{
			return !(left >= right);
		}
		
		/// <summary>
		/// Greater or equall comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator >= (IPAddressWrapper left, IPAddressWrapper right)
		{
			return left > right || left == right;
		}
		
		/// <summary>
		/// Not equall comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator != (IPAddressWrapper left, IPAddressWrapper right)
		{
			return !(left == right);
		}
		
		/// <summary>
		/// Less or equal comparer
		/// </summary>
		/// <param name="left">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <param name="right">
		/// A <see cref="IPAddressWrapper"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool operator <= (IPAddressWrapper left, IPAddressWrapper right)
		{
			return !(left > right);
		}
		
		/// <summary>
		/// Gets the hash code for this instance
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public override int GetHashCode ()
		{
			return this.ipAddr.GetHashCode();
		}

		/// <summary>
		/// Compares two instances
		/// </summary>
		/// <param name="obj">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public override bool Equals (object obj)
		{
			if (!(obj is IPAddressWrapper))
				return false;
				
			IPAddressWrapper wrap = (IPAddressWrapper)obj;

			if (wrap == this)
				return true;
            
			if (wrap.ipAddr == this.ipAddr)
				return true;
			if (this.ipAddr.Equals(wrap.ipAddr))
				return true;
			
			return false;
		}
	}
}
