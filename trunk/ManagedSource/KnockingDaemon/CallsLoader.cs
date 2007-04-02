
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;

namespace SharpKnocking.KnockingDaemon
{
	
	/// <summary>
	/// 
	/// </summary>
	public class CallsLoader
	{
	
		/// <summary>
		/// This method loads the defined call sequences.
		/// </summary>
		/// <returns>
		/// The secuences defined in the config file.
		/// </returns>
		public static CallSequence[] Load()
		{			
			DoormanConfig config = DoormanConfig.Load();			
			ArrayList calls = new ArrayList();
						
			foreach (CallSequence seq in config.CallSequences)
			{
				if(config.GetActivationStatus(seq))
				{
					calls.Add(seq);	
				}
			}
			
			return (CallSequence[]) (calls.ToArray(typeof(CallSequence)));
		}
	}
}
