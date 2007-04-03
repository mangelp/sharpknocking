
using System;

using SharpKnocking.Common.Calls;

namespace SharpKnocking.Doorman.Remoting
{
	/// <summary>
	/// The delegate type for the handlers of DaemonCommunication's AccessRequest event.
	public delegate void AccessRequestEventHandler(object sender, AccessRequestEventArgs a);
	
	public class AccessRequestEventArgs : EventArgs
	{
		private CallSequence call;
		
		public AccessRequestEventArgs(string xmlCall)
		{
			call = CallSequence.LoadFromString(xmlCall);
		}
	}
}
