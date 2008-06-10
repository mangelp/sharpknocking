// ConversionTests.cs
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

using NUnit.Core;
using NUnit.Framework;

using Developer.Common.Types;

namespace Tests
{
	
	[TestFixture]
	public class ConversionTests
	{
		
		public void ToByteArray()
		{
			long num1 = 0xABCDABCD;
			int num2 = 0xABCD;
			short num3 = 0xAB;
			
			byte[] result = Conversion.ToByteArray(num1);
			Assert.AreEqual(result.Length, 8, "Failed conversion from long to byte array");
			long numa = Conversion.ToLong(result);
			Assert.AreEqual(numa, num1, "Failed conversion from byte array to long");
			
			result = Conversion.ToByteArray(num2);
			Assert.AreEqual(result.Length, 4, "Failed conversion from int to byte array");
			int numb = Conversion.ToInt(result);
			Assert.AreEqual(numb, num2, "Failed conversion from byte array to int");
			
			result = Conversion.ToByteArray(num3);
			Assert.AreEqual(result.Length, 4, "Failed conversion from int to byte array");
			int numc = Conversion.ToInt(result);
			Assert.AreEqual(numc, num3, "Failed conversion from byte array to short");
		}
		
		public void Test2()
		{}
	}
}
