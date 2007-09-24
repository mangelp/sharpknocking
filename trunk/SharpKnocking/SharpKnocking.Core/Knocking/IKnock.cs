// SharpKnocking/SharpKnocking.Core/Network/IPacket.cs
//
//  Copyright (C) 2007 Miguel Ángel Pérez y Luis Román Gutiérrez
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
using Developer.Common.Net;

namespace SharpKnocking.Core.Knocking
{
	/// <summary>
	/// Models all the data and operations with a captured packet
	/// </summary>
	/// <remarks>
	/// </remarks>
	public interface IKnock
	{
		IpEndPoint Source {get;set;}
		IpEndPoint Target {get;set;}
		DateTime TimeStamp {get;set;}
		byte[] Data {get;set;}
		ProtocolType Type {get;set;}
		object CustomData {get;set;}
	}
}
