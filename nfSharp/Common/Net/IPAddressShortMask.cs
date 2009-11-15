// IPAddressShortMask.cs
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

using NFSharp.Common.Types;

namespace NFSharp.Common.Net
{
	/// <summary>
	/// Models a short mask format for an ip address (v4 or v6) specifying
	/// what bytes are part of the mask starting from the most significative one.
	/// </summary>
	public class IPAddressShortMask: IPAddressMask
	{
		/// <summary>
		/// Get/set the mask as an integer value with the number of bits from
		/// the left masked.
		/// </summary>
		public int Mask
		{
			get {
				return base.ShortMask;
			}
			
			set {
				base.ShortMask = value;
			}
		}
		
		/// <summary>
		/// Creates a new instance with a default mask value
		/// </summary>
		/// <remarks>The default mask value is 0</remarks>
		public IPAddressShortMask()
		{
			base.ShortMask = 0;
		}
		
		/// <summary>
		/// Creates a new instance with a mask value
		/// </summary>
		/// <param name="mask">
		/// A <see cref="System.Int32"/>
		/// </param>
		public IPAddressShortMask(int mask)
		{
			base.ShortMask = mask;
		}
		
		/// <summary>
		/// Applies the mask to an address byte array
		/// </summary>
		/// <param name="bytes">
		/// A <see cref="System.Byte"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Byte"/>
		/// </returns>
		protected override byte[] ApplyMask (byte[] bytes)
		{
			if (bytes.Length*8 < this.Mask)
				throw new ArgumentException("Mask length is greater than the number of"+
				                            "address bits", "bytes");
			long mask = -1L;
			mask = mask >> this.Mask;
			byte[] maskArr = Conversion.ToByteArray(mask);
			
			for(int i=0; i<8; i++) {
				bytes[i] = (byte)(bytes[i] & maskArr[i]);
			}
			
			return bytes;
		}
		
		/// <summary>
		/// Returns the current mask as a long mask
		/// </summary>
		/// <returns>
		/// A <see cref="IPAddressLongMask"/>
		/// </returns>
		public IPAddressLongMask AsLongMask()
		{
			long val = -1L;
			val = val >> this.Mask;
			return new IPAddressLongMask(val);
		}
	}
}
