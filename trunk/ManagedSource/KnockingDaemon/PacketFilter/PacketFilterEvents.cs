
using System;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	public delegate void PacketCapturedEventHandler(
		object sender,
		PacketCapturedEventArgs a);
	
	/// <summary>
	/// This class extends <c>EventArgs</c> so it can be used to 
	/// inform when a valid packet is captured.
	/// </summary>
	public class PacketCapturedEventArgs
		: EventArgs
	{
		private PacketInfo info;
			
		public PacketCapturedEventArgs(PacketInfo created)
			: base()
		{
			info = created;
		}
		
		#region Properties
		
		/// <summary>
		/// This property allows its user to retrieve the information got about
		/// the captured packet.
		/// </summary>
		public PacketInfo Packet
		{
			get
			{
				return info;
			}
		}	
		
		#endregion Properties 
	}
	
	
}
