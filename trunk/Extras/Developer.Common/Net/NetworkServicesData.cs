
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Developer.Common.Types;

namespace Developer.Common.Net
{
	/// <summary>
	/// This static class loads the data in the file /etc/services and keep it in
	/// memory
	/// </summary>
	/// <remarks>
	/// Based in the class PortInverseResolver written by luis <luisgz@gmail.com>. 
	/// </remarks>
	public class NetworkServicesData
	{
		private static Dictionary<string,NetworkService> dictionary;
		
		static NetworkServicesData()
		{
			//There are almot 9000 lines in the /etc/services file under fc6 so
			//is a good idea to start the dictionary with this size
			dictionary = new Dictionary<string,NetworkService>(9000);
			LoadData();
		}
		
		#region Public methods
		private static void LoadData()
		{
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
					
					if(!AliasUtil.IsAliasName(typeof(ProtocolType),parts[1],out val))
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
		
		#endregion Public methods.
	}
}
