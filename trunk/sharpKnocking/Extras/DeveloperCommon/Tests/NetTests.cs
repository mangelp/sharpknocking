// NetTests.cs
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

using Developer.Common;
using Developer.Common.Net;

using NUnit.Core;
using NUnit.Framework;

namespace Developer.Common.Tests
{
	[TestFixture]
	public class NetTests
	{
		public void ParseAddressAndMask()
		{
			IPAddressWrapper ipaddr = IPAddressWrapper.Parse("192.168.1.20/24");
			Console.WriteLine("Converted 192.168.1.20/24");
			Assert.IsNotNull(ipaddr.IpAddress, "Must be not null!");
			Assert.IsNotNull(ipaddr.Mask, "Must be not null!");
		}
	}
}
