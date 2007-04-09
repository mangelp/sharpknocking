
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
		private CallSequence call;
		
		private string originIP;
		
		public AccessRequestEventArgs(string data)
		{
			// We separate the xml representation of the CallSequence from
			// the source ip with a <>, which cannot be present in the xml part.
			
			int idx = data.LastIndexOf("<>");
			string xml = data.Substring(0,idx);
			originIP = data.Substring(idx + 2);
			
			Console.WriteLine("Datos recibidos {0}, {1}", xml,originIP);
			call = CallSequence.LoadFromString(xml);
			
		}
		
		/// <summary>
		/// Allows retrieving the sequence which was detected.
		/// </summary>
		public CallSequence CallSequence
		{
			get
			{
				return call;
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
				return originIP;
			}
		}
		
	}
}
