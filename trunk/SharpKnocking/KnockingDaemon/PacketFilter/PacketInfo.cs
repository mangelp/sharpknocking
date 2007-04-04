
using System;
using System.Net;
using System.Globalization;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	/// <summary>
	/// This class implements a storage for the information given extracted by
	/// TcpdumpMonitor.
	/// </summary>
	public class PacketInfo
	{
		#region Attributes
		
		private IPAddress sourceAddress;
		private string time;
		private int port;
		private int order;
		
		
		#endregion Attributes
		
		
		/// <summary>
		/// PacketInfo's constructor.
		/// </summary>
		public PacketInfo()
		{
		}
		
		#region Properties		
		
		/// <summary>
		/// The packet's destination port can be retrieved through this property.
		/// </summary>
		public int DestinationPort
		{
			get
			{
				return port;
			}
		}
		
		/// <summary>
		/// This property allows the user to retrive the "sequence number" of the 
		/// received packet.
		/// </summary>
		public int Order
		{
			get
			{
				return order;
			}
		}
		
		/// <summary>
		/// This property is used to retrieve the packet's source address.
		/// </summary>
		public IPAddress SourceAddress
		{
			get
			{
				return sourceAddress;
			}
		}
		
		/// <summary>
		/// This property is used to retrieve the packet's arrival time.
		/// </summary>
		public string ArrivalTime
		{
			get
			{
				return time;
			}
		}	

		#endregion Properties
		
		#region Public methods
		
		
		public static PacketInfo Parse(
			string arrivalTime,
			string destinationPort,
			string order,
			string sourceAddress)
		{
			PacketInfo packet = new PacketInfo();
			
			try
			{
				packet.port = Int32.Parse(destinationPort);				
				packet.order = Int32.Parse(order, NumberStyles.AllowHexSpecifier);			
				packet.time = arrivalTime;				
				
				packet.sourceAddress = IPAddress.Parse(sourceAddress); 			
			}
			catch(Exception e)
			{
				packet = null;
				Console.WriteLine(e.Message);
			}
			
			return packet;
		}
		
		public override string ToString ()
		{
			string res = String.Format(
				"Packet from {0}, with order {1}, targeted at {2} port, arrived at {3}",
				sourceAddress,
				order,
				port,
				time);
				
			return res;
		}


		#endregion Public methods
	}
}
