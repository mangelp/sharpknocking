
using System;

using SharpKnocking.Common.Calls;

namespace SharpKnocking.Doorman.Remoting
{
	/// <summary>
	/// The delegate type for the handlers of DaemonCommunication's
	/// <c>AccessRequest</c> event.
	/// </summary>
	public delegate void AccessRequestEventHandler(object sender, AccessRequestEventArgs a);
	
	/// <summary>
	/// The class which implements the arguments for the <c>AccessRequest</c> event.
	/// </summary>
	public class AccessRequestEventArgs : EventArgs
	{
		private CallHitData data;	
		
		public AccessRequestEventArgs(CallHitData data)
		{
			this.data = data;
			
		}
		
		/// <summary>
		/// Allows retrieving the sequence which was detected.
		/// </summary>
		public CallSequence CallSequence
		{
			get
			{
				return data.SequenceHit;
			}
		}
		
		/// <summary>
		/// Allows retrieving the IP address of the machine which
		/// sent the sequence.
		/// </summary>
		public string SourceIP
		{
			get
			{
				return data.IpAddress;
			}
		}
		
		public CallHitData HitData
		{
			get
			{
				return data;
			}
		}
		
	}
}
