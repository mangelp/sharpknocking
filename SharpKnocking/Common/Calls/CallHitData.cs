
using System;



namespace SharpKnocking.Common.Calls
{	

	/// <summary>
	/// This class encapsulates the info needed to be sent accross applications
	/// to open ports because of having received a call sequence in interactive
	/// mode.
	/// </summary>
	[Serializable]
	public class CallHitData
	{
		private string ip;
			
		private CallSequence sequence;
		
		public CallHitData(string ipAddress, CallSequence sequence)
		{
			this.ip = ipAddress;
			this.sequence = sequence;
		}
		
		/// <summary>
		/// The ip address from which the sequence was sent.
		/// </summary>
		public string IpAddress
		{
			get
			{
				return ip;
			}
		}
		
		/// <summary>
		/// The sequence hit.
		/// </summary>
		public CallSequence SequenceHit
		{
			get
			{
				return sequence;
			}
		}
		
		
	}
}
