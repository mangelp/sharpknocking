
using System;
using System.Net;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.KnockingDaemon.PacketFilter;


namespace SharpKnocking.KnockingDaemon.SequenceDetection
{
	
	/// <summary>
	/// This class implements an automaton to be able of deciding if a call 
	/// sequence has been received.
	/// </summary>
	public class SequenceDetector: IDisposable
	{
		#region Properties
		
		private CallSequence sequence;
		
		//NOTE: There is a severe limitation. There is not a timeout logic that
		//will delete entries when they become old so the hashtable will grow 
		//for ever until the daemon is restarted.
		
		//Number of ports in sequence touched
		private Hashtable hitsTable;

		#endregion Properties
		
		public event SequenceDetectorEventHandler SequenceDetected;
		

		/// <summary>
		/// <c>SequenceDetector</c>'s constructor.
		/// </summary>
		/// <param name = "sequence">
		/// The sequence we will check if it has been triggered.
		/// </param>
		public SequenceDetector(CallSequence sequence)
		{
			this.sequence = sequence;	
			
			if(sequence.Ports.Length==0)
				throw new ArgumentException("The number of ports in the sequence can't be 0!", "sequence");

			this.hitsTable = new Hashtable(15);
		}
		
		#region Properties
		
		/// <summary>
		/// You can retrieve the <c>CallSequence</c> associated to the detector
		/// through this property.
		/// </summary>
		public CallSequence CallSequence
		{
			get
			{
				return sequence;
			}
		}

		#endregion
		
		#region Public methods
		
		/// <summary>
		/// This method takes a packet and check if it is part of the sequence.
		/// </summary>
		/// <param name = "packet">
		/// The packet which will be checked.
		/// </param>
		public void CheckPacket(PacketInfo packet)
		{
		    string sourceAddr = packet.SourceAddress.ToString();
			// If we have it we check the number if not we check the first number
			// and we add if it is correct.
			if(this.hitsTable.ContainsKey (sourceAddr))
			{
			    Debug.VerboseWrite("Existing ip in collection: "+sourceAddr
			             +" port "+packet.DestinationPort, 
			             VerbosityLevels.High);
			    
				int next = (int)this.hitsTable[sourceAddr];
				
				if(packet.DestinationPort == this.CallSequence.Ports[next])
				{
					//Increment expected packet number
					this.hitsTable [sourceAddr] = next + 1;
					
					if((next+1) == this.CallSequence.Ports.Length)
					{
					    Debug.VerboseWrite("Sequence hit for ip: "+sourceAddr, 
					               VerbosityLevels.High);
					    this.hitsTable.Remove(sourceAddr);
					    //A sequence have been completely detected so we must
					    //notify other about it. We serialize the CallSequence
					    //object as an xml
						this.OnSequenceDetectedHelper(
							sourceAddr, 
						    this.CallSequence.Store(), 
						    this.CallSequence.TargetPort);
					}
				}
				else
				{
					//Reset expected packet number					
					this.hitsTable.Remove(sourceAddr);
				}
			}
			else if(packet.DestinationPort == this.CallSequence.Ports[0])
			{
			    Debug.VerboseWrite("Adding ip in collection: "+sourceAddr
			             +" port "+packet.DestinationPort, 
			             VerbosityLevels.High);
				this.hitsTable.Add(this.CallSequence.Address, 1);
			}
		}
		
		#endregion Public methods
		
		#region Private methods
		
		private void OnSequenceDetectedHelper(string ip, string seq, int port)
		{
		    SequenceDetectorEventArgs args = new SequenceDetectorEventArgs(ip, seq, port);
		    
			if(SequenceDetected != null)
				SequenceDetected(this, args);
		}

		#endregion Private methods
		
		public void Dispose()
		{
			this.hitsTable.Clear();
		}
	}
}
