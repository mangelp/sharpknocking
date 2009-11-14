using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;

using SharpKnocking.Common.Calls;

namespace SharpKnocking.PortKnocker
{	
	/// <summary>
	/// This class is used to retrieve and store <c>PortKnocker</c>'s 
	/// configuration information.
	/// </summary>
	public class PortKnockerConfig
	{
		private static string configFilePath = 
		    Environment.GetEnvironmentVariable("HOME") + "/.portknocker";
		
		private ArrayList calls;
		
		public PortKnockerConfig()
		{
			calls = new ArrayList();
		}
		
		#region Properties
		
		public CallSequence[] CallSequences
		{
			get
			{
				return (CallSequence[])(calls.ToArray(typeof(CallSequence)));
			}
			set
			{
				calls = new ArrayList(value);
			}		
		}
		
		#endregion Properties
		
		#region Public methods

		public void AddCall(CallSequence call)
		{
			calls.Add(call);
		}
		
		public static PortKnockerConfig Load()
		{
			if(!File.Exists(configFilePath))
				return null;
			else
			{
				// There is a config file, so we load it.
				XmlSerializer serializer = new XmlSerializer(
												typeof(PortKnockerConfig),
												new Type[]{
													typeof(CallSequence),
													typeof(int)});
				PortKnockerConfig config = null;
				try
				{
					FileStream stream = new FileStream(configFilePath,FileMode.Open);
					config = (PortKnockerConfig)(serializer.Deserialize(stream));
					stream.Close();
				}
				catch(Exception)
				{
					Console.WriteLine("Invalid config file, loading nothing.");
				}
													
				return config;									
			}
		}
		
		public void Save()
		{
			XmlSerializer serializer = new XmlSerializer(
												typeof(PortKnockerConfig),
												new Type[]{
													typeof(CallSequence),
													typeof(int)});
			
			FileStream stream = new FileStream(configFilePath,FileMode.Create);										
			serializer.Serialize(stream,this);
			stream.Close();									
		}
		
		#endregion Public methods
	}
}
