
using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;


using SharpKnocking.Common.Calls;

namespace SharpKnocking.Common
{	
	/// <summary>
	/// This class is used to retrieve and store <c>Doorman</c>'s 
	/// configuration information.
	/// </summary>
	public class DoormanConfig
	{
		// Hardcoding paths is bad! Someone think about the children!
		private static string configFilePath = "/etc/doorman";
		    
		private ArrayList calls;
		private ArrayList activationStatuses;
		
		public DoormanConfig()
		{
			calls = new ArrayList();
			activationStatuses = new ArrayList();
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
		
		public bool[] ActivationStatuses
		{
			get
			{
				return (bool[])(activationStatuses.ToArray(typeof(bool)));
			}
			set
			{
				activationStatuses = new ArrayList(value);
			}
		}
		
		#endregion Properties

		#region Public methods
		
		public void AddCall(CallSequence call, bool status)
		{
			calls.Add(call);
			activationStatuses.Add(status);
		}
		
		public bool GetActivationStatus(CallSequence call)
		{
			int idx = calls.IndexOf(call);
			return (bool)(activationStatuses[idx]);		
		}
		
		public static DoormanConfig Load()
		{
			
			if(!File.Exists(configFilePath))
				return null;
			else
			{
				// There is a config file, so we load it.
				XmlSerializer serializer = new XmlSerializer(
												typeof(DoormanConfig),
												new Type[]{
													typeof(CallSequence),
													typeof(int),
													typeof(bool)});
				DoormanConfig config = null;
				try
				{	
					FileStream stream = 
						new FileStream(configFilePath,FileMode.Open);
						
					config = (DoormanConfig)(serializer.Deserialize(stream));
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
												typeof(DoormanConfig),
												new Type[]{
													typeof(CallSequence),
													typeof(int),
													typeof(bool)});
													
			FileStream stream = new FileStream(configFilePath,FileMode.Create);									
			serializer.Serialize(stream, this);
			stream.Close();
		}

		#endregion Public methods
	}
	
}
