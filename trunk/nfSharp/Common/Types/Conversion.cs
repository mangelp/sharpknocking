// Conversion.cs
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

namespace NFSharp.Common.Types {
    /// <summary>
    /// Usefull conversion methods between uncommon types
    /// </summary>
    public static class Conversion {
        private static byte[] ToByteArray(byte byteNum, long val) {
            byte[] res = new byte[byteNum];
            long mask = 0x0000000F;
            for (int i = byteNum - 1; i >= 0; i--) {
                res[i] = Convert.ToByte(val & mask);
                val = val >> 8;
            }

            return res;
        }

        /// <summary>
        /// Converts a long value to a byte array
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Int64"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte"/>
        /// </returns>
        public static byte[] ToByteArray(long val) {
            return ToByteArray(8, val);
        }

        /// <summary>
        /// Converts an integer to an array of bytes
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte"/>
        /// </returns>
        public static byte[] ToByteArray(int val) {
            return ToByteArray(4, Convert.ToInt64(val));
        }

        /// <summary>
        /// Converts a short value into an array of bytes
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Int16"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte"/>
        /// </returns>
        public static byte[] ToByteArray(short val) {
            return ToByteArray(2, Convert.ToInt64(val));
        }

        /// <summary>
        /// Converts an array of bytes to long
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Byte"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Int64"/>
        /// </returns>
        public static long ToLong(byte[] val) {
            if (val.Length != 8) {
                throw new FormatException("The input array must have 8 bytes");
            }
            long res = 0x00000000;
            for (int i = 0 ; i < val.Length ; i++) {
                res = res | val[i];
                res = res << 8;
            }
            return res;
        }

        /// <summary>
        /// Converts an array of bytes to an integer
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Byte"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        public static int ToInt(byte[] val) {
            return Convert.ToInt32(ToLong(val));
        }

        /// <summary>
        /// Coverts an array of bytes to a short
        /// </summary>
        /// <param name="val">
        /// A <see cref="System.Byte"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Int16"/>
        /// </returns>
        public static short ToShort(byte[] val) {
            return Convert.ToInt16(ToLong(val));
        }
    }
}
