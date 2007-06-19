// /home/mangelp/Projects/sharpknocking/SharpKnocking/Common/Net/Structs.cs created with MonoDevelop at 12:51 12/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;

namespace Developer.Common.Net
{
	public struct NetworkService
	{
		public static readonly NetworkService Empty = new NetworkService();
		
		public string Name;
		public ProtocolType Protocol;
		public UInt16 Port;
		
		public override string ToString ()
		{
			return Name+"@"+Protocol+"/"+Port;
		}
		
		public override int GetHashCode ()
		{
			return this.ToString().GetHashCode();
		}
	}
	
	/// <summary>
	/// Structure for ipv4 sharpknocking packets
	/// </summary>
	public struct SkIpV4Packet
	{
		public IPv4RawAddress Address;
		
		public UInt16 SourcePort;
		public UInt16 DestinationPort;
		
		public ProtocolType Protocol;
		public byte[] Payload;
	}
	
	/// <summary>
	/// Structure for ipv6 sharpknocking packets
	/// </summary>
	public struct SkIpV6Packet
	{
		public IPv6RawAddress Address;
		
		public UInt16 SourcePort;
		public UInt16 DestinationPort;
		
		public ProtocolType Protocol;
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
	
	
	
	
	 /// <summary>
    /// Defines a port range that at least can be a range of 1 port.
    /// </summary>
    public struct PortRange
    {
		/// <summary>
		/// Gets if the range is a valid range with values greater or equal to 0 
		/// and with the end port equal or greater than the start port
		/// </summary>
		public bool IsValid
		{
			get { return this.startPort>=0 && this.endPort>=this.startPort;	}
		}
        
        private short startPort;
        
		/// <summary>
		/// Start port of the range. If the SinglePort property is true this
		/// has the same value as EndPort.
		/// </summary>
        public short StartPort
        {
            get { return this.startPort; }
        }
        
        private short endPort;
        
		/// <summary>
		/// End port of the range. If the SinglePort property is true this
		/// has the same value as EndPort.
		/// </summary>
        public short EndPort
        {
            get { return this.endPort;}
        }
        
        public PortRange(short port)
        {
            this.startPort = port;
            this.endPort = Int16.MaxValue;
        }
        
        public PortRange(short start, short end)
        {
            if(start<0)
                this.startPort = 0;
            else
                this.startPort = start;
            
            if(end<0)
                this.endPort = Int16.MaxValue;
            else
                this.endPort = end;
            
            if(end < start)
            {
                this.startPort = end;
                this.endPort = start;
            }
        }
        
        public override string ToString ()
        {
            return startPort+":"+endPort;
        }
		
		public override bool Equals (object o)
		{
			 if(!(o is PortRange))
				return false;
			
			PortRange range = (PortRange)o;
			
			if(range.startPort!= this.startPort || 
			   range.endPort!= this.endPort)
				return false;
			
			return true;
		}
		
		public override int GetHashCode ()
		{
			return (startPort+":"+endPort).GetHashCode();
		}

		/// <summary>
		/// Parses an string defining a range and assign them
		/// </summary>
		/// <remarks>
		/// A single integer can be a valid range and also two integers separated by
		/// the ':' character.
		/// Ej: 
		/// 5    => Port range from 5 to 5 (only one port)
		/// 5:24 => Port range from 5 to 25
		/// </remarks>
        public static PortRange Parse (string range)
        {
			short endp;
            short startp;
			
            int pos = range.IndexOf (':');
            
            if (pos < 0) {
                //Only first port
                if (!Int16.TryParse(range, out startp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse to integer. Data: "+range);
                }
                endp = Int16.MaxValue;
            } else if (pos == 0) {
                //Only last port
                if (!Int16.TryParse(range.Substring(1), out endp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse to integer. Data: "+range);
                }
                startp = 0;
            } else {
				//Convert the first port
				if (!Int16.TryParse (range.Substring(0,pos),out startp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse start port to integer. Data: "+range);				
				}
				//Convert the last port
				if (!Int16.TryParse (range.Substring(0,pos),out endp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse end port to integer. Data: "+range);				
				}
			}
            
            return new PortRange (startp, endp);
        }
    }
}
