
using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace SharpKnocking.KnockingDaemon
{
	
	/// <summary>
	/// This class is used to translate port protocol names back into numbers.
	/// </summary>
	public class PortInverseResolver
	{
		private static StringDictionary dictionary;
		
		
		public static void LoadTranslations()
		{
			dictionary = new StringDictionary();
			
			using(StreamReader reader = new StreamReader("/etc/protocols"))
			{
				string line;
				while((line = reader.ReadLine()) != null)
				{
					if(!line.StartsWith("#"))
						Console.WriteLine(line);
				}
				
				
				
				reader.Close();
			}
						
		}
	}
}
