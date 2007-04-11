
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.KnockingDaemon.PacketFilter;

namespace SharpKnocking.KnockingDaemon.SequenceDetection
{
	
	/// <summary>
	/// This class manages the process of matching a packet sequence to 
	/// call sequence defined.
	/// </summary>	
	public class SequenceDetectorManager: IDisposable
	{
        #region Events
        
        public event SequenceDetectorEventHandler SequenceDetected;
        
        #endregion
	   
		#region Attributes
		
		private ArrayList detectors;
		private TcpdumpMonitor monitor;

		#endregion Attributes
		
		/// <summary>
		/// <c>SequenceDetectorManager</c>'s constructor.
		/// </summary>
		/// <param name = "sequences">
		/// The secuences which will be monitored.
		/// </param>
		/// <remarks>
		/// The monitor reference is required to handle the <c>PacketCapture</c> 
		/// event and to release it in <c>Dispose</c> implementation. 
		/// </remarks>
		public SequenceDetectorManager(CallSequence [] sequences, TcpdumpMonitor monitor)
		{
			detectors = new ArrayList();
			
			foreach(CallSequence seq in sequences)
				AddSequence(seq);
				
		    this.monitor = monitor;
		    
			monitor.PacketCaptured += 
				new PacketCapturedEventHandler(OnPacketCaptured);
		}
		
		/// <summary>
		/// Clear collections, events handling and references to other objects.
		/// </summary>
		public void Dispose()
		{
		    if(this.detectors !=null)
		    {
		        this.detectors.Clear();
		        this.detectors = null;
		    }
		    
		    if(this.monitor !=null)
		    {
		        this.monitor.PacketCaptured -= 
		            new PacketCapturedEventHandler(OnPacketCaptured);
		        this.monitor = null;
		    } 
		}
		
		#region Public Methods
		
		/// <summary>
		/// This method allows to add a new sequence so it will be monitored.
		/// </summary>
		/// <param name = "sequence">
		/// The <c>CallSequence</c> instance which will be monitored.
		/// </param>
		public void AddSequence(CallSequence sequence)
		{
			// We create a new SequenceDetector and store it.
			SequenceDetector sd = new SequenceDetector(sequence);		
			sd.SequenceDetected += OnSequenceDetected;
			detectors.Add(sd);			
		}
		
		/// <summary>
		/// This method allows you to check if a packet is part of a sequence.
		/// </summary>
		/// <param name = "packet">
		/// The packet which will be checked.
		/// </param>
		public void CheckPacket(PacketInfo packet)
		{
			Debug.Write("SequenceDetectorManager:: Received packet: "+packet);
			
			foreach(SequenceDetector sd in detectors)
			{
				sd.CheckPacket(packet);
			}
		}
		
//		/// <summary>
//		/// This method allow to indicate which object will provide the packets
//		/// which should be checked.
//		/// </summary>
//		/// <param name = "monitor">
//		/// The monitor class' instance.
//		/// </param>
//		public void LinkPacketMonitor(TcpdumpMonitor monitor)
//		{
//			monitor.PacketCaptured += 
//				new PacketCapturedEventHandler(OnPacketCaptured);			
//		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		//Manages the event SequenceDetected from any detector.
		private void OnSequenceDetected(object sender, 
		                  SequenceDetectorEventArgs args)
		{			
			Debug.VerboseWrite("Detected sequence "+
			         ((SequenceDetector)sender).CallSequence);
		    //Rethrow the event but making this class the owner
		    this.OnSequenceDetectedEvent(args);	
		}
		
		//Notifies about the sequenceDetected event from one detector
		private void OnSequenceDetectedEvent(SequenceDetectorEventArgs args)
		{
		    Debug.VerboseWrite ("SequenceDetectorManager:: Sequence detected!. Notifying ...");
			if(this.SequenceDetected != null)
			{
			    this.SequenceDetected(this, args); 
			}
		}
		
		private void OnPacketCaptured(object sender, PacketCapturedEventArgs a)
		{
		    Debug.VerboseWrite ("SequenceDetectorManager:: Packet captured. Analizing ...");
			// When a packet is captured, we check the packet
			CheckPacket(a.Packet);
		}
		
		#endregion Private methods
	}
}
