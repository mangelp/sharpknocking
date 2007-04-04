
using System;
using System.Timers;
using System.Collections;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	/// <summary>
	/// This class' task is to create a packet from the info readed by 
	/// TcpdumpMonitor.
	/// </summary>
	public class PacketAssembler
	{
		#region Attributes
		
		private ArrayList lineBuffer;
		
		private Timer timer;
		
		private bool processingPacket;
		
		#endregion Attributes
		
		#region Events
		
		public event PacketCapturedEventHandler PacketCaptured;

		#endregion Events
		
		/// <summary>
		/// PacketAssembler's default constructor.
		/// </summary>	
		public PacketAssembler()
		{
			InitTimer();
			
			
			lineBuffer = new ArrayList();		
		}
		
		#region Public methods		
		
		/// <summary>
		/// This method takes a text line and extract information from it so
		/// we can create a packet.
		/// </summary>
		public void AddLine(string line)
		{
			// The timer is stopped.
			timer.Enabled = false;
			
			string [] linePieces = line.Split(' ');
			
			bool isHeader = IsHeaderLine(linePieces);	
		
			if(isHeader)
			{		
				// If it is a header, we consider the previous package 
				// is complete.				
				AssemblePackage();
				
				lineBuffer.Add(linePieces);	
				
				processingPacket = true;
			}
			else if(processingPacket)
			{
				lineBuffer.Add(linePieces);
				// We only enable the timer if we have received 
				// a non-header line.
				InitTimer();
				timer.Enabled = true;
					
			}
		}

		#endregion Public methods
		
		#region Private methods
		
		private void AssemblePackage()
		{	
					
			if(lineBuffer.Count == 4)
			{	
				string [] line = (string [])lineBuffer[0];
				
				string port = line[4].Split('.',':')[4];				
				string sourceAddress = line[2].Substring(0, line[2].Length - 6);
				string time = line[0];
				
				line = (string []) lineBuffer[2];
				
				string order =  line[8].Substring(0,2);
								
				PacketInfo packet = PacketInfo.Parse(
					time,
					port,
					order,
					sourceAddress); 
					
				if(packet != null)
				{
					OnPacketAssembledHelper(packet);
				}
								
			}
			
			// We clean the buffer.
			lineBuffer.Clear();
			
			processingPacket = false;
		}	
		
		private void InitTimer()
		{
			timer =  new Timer(500);
			
			timer.AutoReset = true;
			timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
		}
		
		private bool IsHeaderLine(string [] linePieces)
		{
			bool res =  linePieces.Length == 8
						&& linePieces[1] == "IP"
						&& linePieces [3] == ">"
						&& linePieces [5] == "UDP,"
						&& linePieces [7] == "8";
			return res; 
		}
		
		public void OnPacketAssembledHelper(PacketInfo packet)
		{
			if(PacketCaptured != null)
				PacketCaptured(this, new PacketCapturedEventArgs(packet));
		}

		
		private void OnTimerElapsed(object sender, ElapsedEventArgs a)
		{
			Console.WriteLine("timeout!");
			
			
			timer.Enabled = false;
			
			AssemblePackage();
		}
		#endregion Private methods
	}
}
