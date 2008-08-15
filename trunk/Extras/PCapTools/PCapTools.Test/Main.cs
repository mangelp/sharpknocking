// Main.cs
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

namespace PCapTools.Test
{
	//code got from: http://www.chrishowie.com/pcap-sharp/
	class MainClass
	{
		public static void Main(string[] args)
		{
		    using (PcapHandle ph = Pcap.OpenLive(args[0], short.MaxValue, true, 5000)) {
		      // Poll-driven model
		      for (int i = 0; i < 10; i++) {
		        Packet p = ph.ReadNext();
		        Console.WriteLine(p.RealLength);
		      }
		
		      int j = 0;
		      // Callback-driven model
		      ph.Loop(-1, delegate(Packet p) {
		        Console.WriteLine(p.RealLength);
		        if (++j == 10) {
		          ph.BreakLoop();
		        }
		      });
			}
		}
	}
}