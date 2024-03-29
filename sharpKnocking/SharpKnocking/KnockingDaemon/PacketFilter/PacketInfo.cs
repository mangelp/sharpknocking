
using System;
using System.Net;
using System.Globalization;

using SharpKnocking.Common;
using SharpKnocking.KnockingDaemon;

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
			
			bool error = false;
			string msg = String.Empty;
			
			//Port conversion
			try
			{
				packet.port = Int32.Parse(destinationPort);
			}
			catch(FormatException)
			{
				// We try to look in the dictionary.
				string port = PortInverseResolver.Translate(destinationPort);
				
				if(Net20.StringIsNullOrEmpty(port))
				{
					error = true;
					msg = "Can't resolve port "+destinationPort;
				}
				else
				{
					packet.port = Int32.Parse(port);
				}
			}	
			
			//Time check
			try
			{
				packet.order = Int32.Parse(order, NumberStyles.AllowHexSpecifier);			
				packet.time = arrivalTime;				
			}
			catch(Exception ex)
			{				
				error = true;
				msg = "Error converting port or time: "+ex.Message; 
			}
			
			try
			{
			    packet.sourceAddress = IPAddress.Parse(sourceAddress); 
			}
			catch(Exception)
			{
			   string[] pieces = Net20.StringSplit(sourceAddress, true, '.');
			   
			   if(pieces.Length >=4)
			   {
			       sourceAddress = pieces[0]+"."+pieces[1]+"."+pieces[2]+"."+pieces[3];
			       try
			       {
			             packet.sourceAddress = IPAddress.Parse(sourceAddress);   
			       }
			       catch(Exception exe)
			       {
			             msg = "Wrong address format: "+sourceAddress+", Detail: "+exe.Message;      
			       }
			   }
			   else
			     msg = "Wrong address format: "+sourceAddress;
			}
			
			if(error)
			{
				packet = null;
				Debug.VerboseWrite("Malformed packet received. Detail: "+msg);
			}
			else
			{
			    Debug.VerboseWrite("Package assembled!");
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
