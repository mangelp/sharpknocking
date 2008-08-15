// TcpKnocking.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Diego Campoy Collado manrash[at)gmail(doot]com
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

namespace PcapTools.Capturer
{
	public class TcpKnocking
	{
		//TODO: Put all fields in place of only the important.
		//TODO: Check ipv4 format is equal ipv6
		
		// --- TCP level fields ---
		short tcpSrcPort; // 16 bits
		short tcpDstPort; // 16 bits
		int tcpSequenceNumber; // 32 bits
		int tcpAckNumber; // 32 bits
		// Data offset, reserved, type
		short tcpWindow; // 16 bits
		short tcpChecksum; // 16 bits
		short tcpUrgentPointer; // 16 bits
		// Options and padding
		
		// --- IP level fields ---
		byte ipVersion; // 4 bits, thus the first four digit are wasted
		byte ipIHL; // 4 bits, thus the first four digit are wasted
		byte ipTOS; // 8 bits.
		byte ipTotalLenght; // 8 bits
		short ipIdentification; // 16 bits
		// Flags - Fragment Offset
		byte ipTTL; // 3 bits
		byte protocol; // 8 bits
		
		// --- MAC level fields ---
		// TODO
		
		public TcpKnocking()
		{
		}
	}
}