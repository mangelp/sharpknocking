
using System;

namespace SharpKnocking.KnockingDaemon.SequenceDetection
{
    /// <summary>
    /// Delegate for events related to sequence detection
    /// </summary>
    public delegate void SequenceDetectorEventHandler(object sender, SequenceDetectorEventArgs args);
	
	
	/// <summary>
	/// Arguments for events related to sequence detection.
	/// </summary>
	public class SequenceDetectorEventArgs: EventArgs
	{
	   private string ip;
	   
	   /// <summary>
	   /// Ip address of the packet
	   /// </summary>
	   public string IP
	   {
	       get { return this.ip;}
	   }
	   
	   /// <summary>
	   /// Parametrized constructor.
	   /// </summary>
	   public SequenceDetectorEventArgs(string ip)
	   {
	       this.ip = ip;
	   }
	}
}
