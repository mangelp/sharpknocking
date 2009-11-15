// IpAddrMask.cs
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

using NFSharp.Common.Types;

namespace NFSharp.Common.Net
{
	/// <summary>
	/// Models a mask for ip addresses
	/// </summary>
	public abstract class IPAddressMask
	{
		private IPAddressMaskType maskType;
		
		/// <summary>
		/// Mask type
		/// </summary>
		public IPAddressMaskType MaskType
		{
			get { return this.maskType;}
			private set { this.maskType = value;}
		}
		
		private bool useLongMask;
		
		/// <summary>
		/// Gets if the mask is in long format
		/// </summary>
		protected bool UseLongMask
		{
			get { return useLongMask; }
		}
		
		
		private bool useShortMask;
		
		/// <summary>
		/// Gets if the mask in in short format
		/// </summary>
		public bool UseShortMask
		{
			get { return useShortMask; }
		}
		
	    private IPAddress longMask;
	    
	    /// <summary>
	    /// Gets/sets the mask as a ip address.
	    /// </summary>
	    /// <remarks>
	    /// The mask is a ip address used to define what bits are part of the
	    /// network address and what not. This is done setting these bits to
	    /// 1 and the rest to 0. A bit by bit logical and operation between
	    /// the mask and a address will show the network address.
	    /// </remarks>
	    protected IPAddress LongMask
	    {
	        get { return this.longMask;}
	        set { 
				this.longMask = value;
				this.useLongMask = true;
				this.useShortMask = false;
				if (value.GetAddressBytes().Length == 4)
					this.maskType = IPAddressMaskType.Ipv4Long;
				else
					this.maskType = IPAddressMaskType.Ipv6Long;
			}
	    }
	    
	    private int shortMask;
	    
	    /// <summary>
	    /// Gets/sets the mask as the number of bits (counting from the left)
	    /// that are used in the mask (set to 1)
	    /// </summary>
	    protected int ShortMask
	    {
	        get {return this.shortMask;}
	        set
	        {
	            if(value < 0 || value > 64)
	                throw new ArgumentException("Invalid value "+
	                        value+" for the mask. It must be between 0 and 32");
	            
	            this.shortMask = value;
				this.useShortMask = true;
				this.useLongMask = false;
				if (value <= 32)
					this.maskType = IPAddressMaskType.Ipv4Short;
				else
					this.maskType = IPAddressMaskType.Ipv6Short;
	        }
	    }
		
		/// <summary>
		/// Applies the current mask to a byte array that represents an ip address of
		/// 4 (ipv4) or 8 (ipv6) bytes
		/// </summary>
		/// <param name="bytes">
		/// A <see cref="System.Byte"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Byte"/>
		/// </returns>
		/// <exception cref="System.ArgumentException">If the input parameter has an
		/// invalid length</exception>
		protected abstract byte[] ApplyMask(byte[] bytes);
		
		/// <summary>
		/// Returns the ip address resulting of applying the mask to it
		/// </summary>
		/// <param name="addr">
		/// A <see cref="IPAddress"/>
		/// </param>
		/// <returns>
		/// A <see cref="IPAddress"/>
		/// </returns>
		public IPAddress ApplyMask(IPAddress addr)
		{
			byte[] bytes = this.ApplyMask(addr.GetAddressBytes());
			return new IPAddress(bytes);
		}
		
		/// <summary>
		/// Tries to detect the type of mask from the format.
		/// </summary>
		/// <param name="mask">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="IPAddressMaskType"/>
		/// </returns>
		public static IPAddressMaskType GetMaskType(string mask)
		{
			if (String.IsNullOrEmpty(mask))
				throw new ArgumentException("Can't be null or empty", "mask");
			
			int dotCount = 0;
			int doubleDotCount = 0;
			
			for (int i=0; i<mask.Length; i++) {
				if (mask[i] == '.')
					dotCount ++;
				if (mask[i] == ':')
					doubleDotCount ++;
			}
			
			if ((dotCount > 0 && doubleDotCount > 0) || 
			    (dotCount != 3 && dotCount != 0) || 
			    (doubleDotCount != 7 && doubleDotCount != 0) ||
			    (dotCount == 0 && doubleDotCount == 0 && mask.Length != 2))
				return IPAddressMaskType.None;
			
			if (dotCount == 3)
				return IPAddressMaskType.Ipv4Long;
			else if (doubleDotCount == 7)
				return IPAddressMaskType.Ipv6Long;
			
			int number = 0;
			if(!Int32.TryParse(mask, out number))
				return IPAddressMaskType.None;
			
			if (number<=32)
				return IPAddressMaskType.Ipv4Short;
			else if (number <= 64)
				return IPAddressMaskType.Ipv6Short;

			return IPAddressMaskType.None;
		}
		
		/// <summary>
        /// Creates a new IPAddressMask from a string.
        /// </summary>
		public static IPAddressMask Parse(string mask)
		{
		    IPAddressMaskType maskType = IPAddressMask.GetMaskType(mask);
		    
		    IPAddressMask result = null;

			switch(maskType)
			{
				case IPAddressMaskType.Ipv4Long:
				case IPAddressMaskType.Ipv6Long:
					result = new IPAddressLongMask();
					((IPAddressLongMask)result).Mask = IPAddress.Parse(mask);
					break;
				case IPAddressMaskType.Ipv4Short:
				case IPAddressMaskType.Ipv6Short:
					int num = Int32.Parse(mask);
					result = new IPAddressShortMask(num);
					break;
				default:
					throw new FormatException("Invalid mask format: " + mask);
			}
		    
		    return result;
		}
		
		/// <summary>
		/// Converts an string into a mask
		/// </summary>
		/// <param name="mask">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="ipAddrMask">
		/// A <see cref="IPAddressMask"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool TryParse(string mask, out IPAddressMask ipAddrMask)
		{
			ipAddrMask = null;
			try {
				ipAddrMask = IPAddressMask.Parse(mask);
				return true;
			} catch(Exception) {
				return false;
			}
		}
	}
}

