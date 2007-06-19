// /home/mangelp/Projects/sharpknocking/SharpKnocking/Common/Net/Structs.cs created with MonoDevelop at 12:51 12/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;

namespace Common
{
	/// <summary>
	/// Structure for ipv4 sharpknocking packets
	/// </summary>
	public struct SkIpV4Packet
	{
		public IPv4RawAddress Address;
		
		public int SourcePort;
		public int DestinationPort;
		
		public string ProtocolType;
		public byte[] Payload;
	}
	
	/// <summary>
	/// Structure for ipv6 sharpknocking packets
	/// </summary>
	public struct SkIpV6Packet
	{
		public IPv6RawAddress Address;
		
		public int SourcePort;
		public int DestinationPort;
		
		public string ProtocolType;
		public byte[] Payload;
	}
	
	/// <summary>
	/// Raw data struct for ipv4. Stores ip and mask data.
	/// </summary>
	public struct IPv4RawAddress
	{
		/// <summary>
		/// Defines a IPv4RawAddress with all the bytes set to zero.
		/// </summary>
		public static readonly IPv4RawAddress Zero = new IPv4RawAddress(new byte[]{0,0,0,0},new byte[]{0,0,0,0});
		
		/// <summary>
		/// IP address (4 bytes)
		/// </summary>
		public byte[] Address;
		
		/// <summary>
		/// Mask (4bytes)
		/// </summary>
		public byte[] Mask;
		
		/// <summary>
		/// Returns if there is a valid mask defined
		/// </summary>
		public bool HasMask
		{
			get { return Mask[3]==0 && Mask[2]==0 && Mask[1]==0 && Mask[0]==0;}
		}
		
		/// <summary>
		/// Initializes the struct with the bytes for the address.
		/// </summary>
		public IPv4RawAddress(byte[] addr)
		{
			if(addr.Length!=4)
				throw new ArgumentException("The array must have 4 bytes","addr");
			this.Address = addr;
			this.Mask = new byte[4]{0,0,0,0};
		}
		
		/// <summary>
		/// Initializes the struct with the bytes for the address and the mask
		/// </summary>
		public IPv4RawAddress(byte[] addr, byte[] mask)
		{
			if(addr.Length!=4)
				throw new ArgumentException("The array must have 4 bytes","addr");
			if(mask.Length!=4)
				throw new ArgumentException("The array must have 4 bytes","mask");
			this.Address = addr;
			this.Mask = mask;
		}
		
		/// <summary>
		/// Returns an string that represents the data.
		/// </summary>
		public string GetAddressAsString()
		{
			return Address[3]+"."+Address[2]+"."+Address[1]+"."+Address[0]+
				(this.HasMask?
					"/"+Mask[3]+"."+Mask[2]+"."+Mask[1]+"."+Mask[0]
					:
					String.Empty);
		}
		
		/// <summary>
		/// Returns an string that represents the IP address after the mask have
		/// been applied over it. This is, the network address.
		/// </summary>
		public string GetMaskedAddressAsString()
		{
			return (Address[3] & Mask[3]) + "." + (Address[2] & Mask[2]) + "." +
				(Address[1] & Mask[1]) + "." + (Address[0] & Mask[0]);
		}
		
		/// <summary>
		/// Returns an array that represents the IP address after the mask have
		/// been applied over it. This is, the network address.
		/// </summary>
		public byte[] GetMaskedAddressAsArray()
		{
			return new byte[]{(byte)(Address[3] & Mask[3]) , (byte)(Address[2] & Mask[2]) ,
				(byte)(Address[1] & Mask[1]) , (byte)(Address[0] & Mask[0])};
		}
		
		/// <summary>
		/// Sets the mask from the number of bits, starting to count in the msb, that
		/// are set to 1 in the mask.
		/// </summary>
		public void SetMask(int numbits)
		{
			if(numbits<0 || numbits>32)
				throw new ArgumentException("The number of bits in the mask are from 0 to 32");
			
			if(numbits<=8)
				Mask = new byte[]{this.GetBitsValue(numbits),0,0,0};
			else if(numbits<=16)
				Mask = new byte[]{255,this.GetBitsValue(numbits-8),0,0};
			else if(numbits<=24)
				Mask = new byte[]{255,255,this.GetBitsValue(numbits-8),0};
			else if(numbits<=32)
				Mask = new byte[]{255,255,255,this.GetBitsValue(numbits-8)};
		}
		
		/// <summary>
		/// Convert a number of bits set to 1, starting in the msb of a group of
		/// 8 bits, to a decimal value taking every bit not specified as 0.
		/// </summary>
		private byte GetBitsValue(int num)
		{
			if(num==8)
				return 255;
			else if(num==0)
				return 0;
			else if(num>8 || num<0)
				throw new ArgumentException("The number of bits must be in the range 0-8","num");
			
			int max = 7;
			int val = 0;
			while(num>0)
			{
				val+= 2^(max--);
				num--;
			}
			return Convert.ToByte(num);
		}
	}
	
	/// <summary>
	/// TODO: Raw data struct for ipv6.
	/// </summary>
	public struct IPv6RawAddress
	{
		
	}
}
