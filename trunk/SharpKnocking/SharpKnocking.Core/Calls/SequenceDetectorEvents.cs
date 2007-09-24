
using System;

namespace SharpKnocking.Core.Calls
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
	   /// Ip address of the packets from which the sequence was sent.
	   /// </summary>
	   public string IP
	   {
	       get { return this.ip;}
	   }
	   
	   private CallSequence sequence;
	   
	   /// <summary>
	   /// The secuence captured.
	   /// </summary>
	   public CallSequence Sequence
	   {
	   		get
	   		{
	   			return this.sequence;
	   		}
	   }
	   
	   private string serializedSequence;
	   
	   /// <summary>
	   /// Sequence that was hit serialzed as an string.
	   /// </summary>
	   public string SerializedSequence
	   {
	       get { return this.serializedSequence;}
	   }
	   
	   private int port;
	   
	   /// <summary>
	   /// Port number
	   /// </summary>
	   public int Port
	   {
	       get { return port;}
	       set { this.port = value;}
	   }
	   
	   /// <summary>
	   /// Parametrized constructor.
	   /// </summary>
	   /// <param name="ip">Ip address that did the knocking</param>
	   /// <param name="seq">CallSequence serialized as an string</param>	   
	   public SequenceDetectorEventArgs(string ip, CallSequence seq)
	   {
	       this.ip = ip;
	       this.sequence = seq;
	       this.serializedSequence = sequence.Store();	       
	       this.port = sequence.TargetPort;
	   }
	}
}
