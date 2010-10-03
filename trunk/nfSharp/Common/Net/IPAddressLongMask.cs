// IPAddressLongMask.cs
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

namespace NFSharp.Common.Net {
    /// <summary>
    /// Models a mask for ipv4 or ipv6 addresses using a full address specifying the
    /// bits that are part of the address mask.
    /// </summary>
    public class IPAddressLongMask: IPAddressMask {
        /// <summary>
        /// Address mask
        /// </summary>
        public IPAddress Mask {
            get {
                return base.LongMask;
            } set {
                base.LongMask = value;
            }
        }

        /// <summary>
        /// Creates a new instance with a default mask
        /// </summary>
        /// <remarks>The default mask is 0.0.0.0</remarks>
        public IPAddressLongMask() {
            base.LongMask = new IPAddress(0L);
        }

        /// <summary>
        /// Creates a new instance with a mask
        /// </summary>
        /// <param name="mask">
        /// A <see cref="IPAddress"/>
        /// </param>
        public IPAddressLongMask(IPAddress mask) {
            base.LongMask = mask;
        }

        /// <summary>
        /// Creates a new instance with a mask
        /// </summary>
        /// <param name="mask">
        /// A <see cref="System.Int64"/>
        /// </param>
        public IPAddressLongMask(long mask) {
            base.LongMask = new IPAddress(mask);
        }

        /// <summary>
        /// Applies the mask to the array of bytes
        /// </summary>
        /// <param name="bytes">
        /// A <see cref="System.Byte"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte"/>
        /// </returns>
        protected override byte[] ApplyMask (byte[] bytes) {
            byte[] mask = this.Mask.GetAddressBytes();
            if (mask.Length != bytes.Length)
                throw new ArgumentException("Mask byte length is not equal to "+
                                            "address byte length", "bytes");

            for(int i=0; i<mask.Length; i++) {
                bytes[i] &= mask[i];
            }

            return bytes;
        }

    }
}
