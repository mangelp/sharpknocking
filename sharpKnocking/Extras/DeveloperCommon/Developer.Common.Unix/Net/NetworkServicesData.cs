// NetworkServicesData.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez, mangelp@gmail.com
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

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Developer.Common.Types;

namespace Developer.Common.Unix.Net
{
	/// <summary>
	/// This static class loads the data in the file /etc/services and keeps it in
	/// memory
	/// </summary>
	/// <remarks>
	/// Based in the class PortInverseResolver written by Luis Román. 
	/// </remarks>
	public class NetworkServicesData
	{
		private static Dictionary<string,NetworkService> dictionary;
		
		static NetworkServicesData()
		{
			//There are almot 9000 lines in the /etc/services file in fc6 so it
			//is a good idea to start the dictionary with this initial capacity.
			dictionary = new Dictionary<string,NetworkService>(9000);
			LoadData();
		}
		
		/// <summary>
		/// Loads the data from the file /etc/services. This is the common path across
		/// distros but spect and exception if someone moved the file in his distro.
		/// </summary>
		private static void LoadData()
		{
			//If the file isn't in the common place we can't do nothing
			if(!File.Exists("/etc/services"))
				return;
			
			using(StreamReader reader = new StreamReader("/etc/services"))
			{
				string line;
				string [] parts;
				NetworkService ns;
				object val;
				while((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					
					if(line.StartsWith("#"))
						continue;
					
					// Info lines have the following format
					// tftp            69/udp		#whatever
					parts = line.Split('\t',' ');
					
					ns = new NetworkService();
					ns.Name = parts[0];
					parts = parts[1].Split('/');
					
					if(!ushort.TryParse(parts[0],out ns.Port))
						//We found a bad definition. Skip it and continue.
						continue;
					
					if(!AliasUtil.IsAliasName(typeof(Protocols),parts[1],out val))
						continue;
					
					dictionary.Add(ns.Name.ToLower(), ns);
				}
				
				reader.Close();
			}
						
		}
		
		/// <summary>
		/// Returns a network service data structure from the name of the service.
		/// </summary>
		/// <returns>
		/// A valid NetworkService structure o NetworkService.Empty if the service
		/// was not found.
		/// </returns>
		public static NetworkService GetNetworkService(string name)
		{
			name = name.ToLower();
			if(dictionary.ContainsKey(name))
				return dictionary[name];
			else
				return NetworkService.Empty;
		}
	}
}
