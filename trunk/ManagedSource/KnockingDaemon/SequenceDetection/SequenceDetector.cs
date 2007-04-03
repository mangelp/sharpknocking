
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
			//If we have it we check the number if not we check the first number
			//and we add if it is correct.
			if(this.hitsTable.ContainsKey (sourceAddr))
			{
				int next = (int)this.hitsTable[sourceAddr];
				
				if(packet.DestinationPort == this.CallSequence.Ports[next])
				{
					//Increment expected packet number
					this.hitsTable [sourceAddr] = next + 1;
					
					if((next+1) == this.CallSequence.Ports.Length)
					{
						this.OnSequenceDetectedHelper(sourceAddr, this.CallSequence.Store ());
					}
				}
				else
				{
					//Reset expected packet number
					this.hitsTable [sourceAddr] = 0;
				}
			}
			else if(packet.DestinationPort == this.CallSequence.Ports[0])
			{
				this.hitsTable.Add(this.CallSequence.Address, 1);
			}
		}
		
		#endregion Public methods
		
		#region Private methods
		
		private void OnSequenceDetectedHelper(string ip, string seq)
		{
		    SequenceDetectorEventArgs args = new SequenceDetectorEventArgs(ip, seq);
		    
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
