
using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using SharpKnocking.Common;

namespace SharpKnocking.KnockingDaemon
{
	
	/// <summary>
	/// This class is used to translate port protocol names back into numbers.
	/// </summary>
	public class PortInverseResolver
	{
		private static StringDictionary dictionary;
		
		#region Public methods
		public static void LoadTranslations()
		{
			dictionary = new StringDictionary();
			ArrayList aux = new ArrayList();
			
			using(StreamReader reader = new StreamReader("/etc/services"))
			{
				string line;
				string [] parts;
				while((line = reader.ReadLine()) != null)
				{
					// Info lines have the following format
					// tftp            69/udp		#whatever

					if(!line.StartsWith("#"))
					{
						// We discard commentaries.
						
						parts = line.Split('\t','/');
						
						aux.Clear();
						foreach(string s in parts)
						{
							if(!Net20.StringIsNullOrEmpty(s))
							{
								aux.Add(s);
							}
						}
						
						if(aux.Count >= 3)
						{
							// It contains protocol name, port, and type (tcp/udp)
							
							// We add the entries to the dictionary.
							dictionary[aux[0] as string] = aux[1] as string;
						}
					}
				}
				
				reader.Close();
			}
						
		}
		
		public static string Translate(string protocolName)
		{
			return dictionary[protocolName];
		}
		
		#endregion Public methods.
	}
}
