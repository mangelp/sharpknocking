
using System;
using SharpKnocking.Common.Calls;

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
	   
	   private string serializedSequence;
	   
	   /// <summary>
	   /// Sequence that was hit serialzed as an string.
	   /// </summary>
	   public string SerializedSequence
	   {
	       get { return this.serializedSequence;}
	   }
	   
	   /// <summary>
	   /// Parametrized constructor.
	   /// </summary>
	   /// <param name="ip">Ip address that did the knocking</param>
	   /// <param name="serSeq">CallSequence serialized as an string</param>
	   public SequenceDetectorEventArgs(string ip, string serSeq)
	   {
	       this.ip = ip;
	       this.serializedSequence = serSeq;
	   }
	}
}
