
using System;
using System.Collections;

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
			Console.WriteLine("Manager recivió {0}",packet);
			
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
		
		private void OnSequenceDetected(object sender, EventArgs a)
		{
			SequenceDetector sd = sender as SequenceDetector;
			
			// TODO: Report the sequence detected
			// One of two ways:
			// - Notify other with an event and let it add the rule
			// - Put here the code to add a rule or call someone else
			//   to add the rule.
			// - If interactive mode request permission to add the rule and
			//   go for one of the above options.
			Console.WriteLine(sd.CallSequence);
		}
		
		private void OnPacketCaptured(object sender, PacketCapturedEventArgs a)
		{
			// When a packet is captured, we check the packet
			CheckPacket(a.Packet);
		}
		
		#endregion Private methods
	}
}
