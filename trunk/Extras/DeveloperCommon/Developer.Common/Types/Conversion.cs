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

namespace Developer.Common.Types
{
	
	public static class Conversion
	{
		private static byte[] ToByteArray(byte byteNum, long val)
		{
			byte[] res = new byte[byteNum];
			long mask = 0x0000000F;
			for (int i = byteNum - 1; i >= 0; i--) {
				res[i] = Convert.ToByte(val & mask);
				val = val >> 8;
			}
			
			return res;		
		}
		
		public static byte[] ToByteArray(long val)
		{
			return ToByteArray(8, val);
		}
		
		public static byte[] ToByteArray(int val)
		{
			return ToByteArray(4, Convert.ToInt64(val));
		}
		
		public static byte[] ToByteArray(short val)
		{
			return ToByteArray(2, Convert.ToInt64(val));
		}
		
		public static long ToLong(byte[] val)
		{
			if (val.Length != 8)
				throw new FormatException("The input array must have 8 bytes");
			long res = 0x00000000;
			for (int i = 0 ; i < val.Length ; i++) {
				res = res | val[i];
				res = res << 8;
			}
			return res;
		}
		
		public static int ToInt(byte[] val)
		{
			return Convert.ToInt32(ToLong(val));
		}
		
		public static short ToShort(byte[] val)
		{
			return Convert.ToInt16(ToLong(val));
		}
	}
}
