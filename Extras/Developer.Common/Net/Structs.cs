// SharpKnocking/Common/Net/Structs.cs created with MonoDevelop at 12:51 12/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;
using System.Net;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Developer.Common.Net
{
	/// <summary>
	/// Represents a network service like those in /etc/services
	/// </summary>
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
	/// Ip end point structure
	/// </summary>
	/// <remarks>
	/// This is already defined in System.Net!
	/// TODO: Delete this and use what the framework provides.
	/// </remarks>
	public struct IpEndPoint
	{
		public int Length;
		public byte[] Addr;
		public UInt16 TargetPort;
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
		private byte[] address;
		
		/// <summary>
		/// Gets/Sets the address bytes (4 bytes).
		/// </summary>
		public byte[] Address
		{
			get {return this.address;}
			set {
				
				if(value.Length!=4)
					throw new ArgumentException("The address must be 4 bytes long","value");
				
				this.address = value;
			}
		}
		
		/// <summary>
		/// Mask (4bytes)
		/// </summary>
		private byte[] mask;
		
		/// <summary>
		/// Gets/Sets the mask bytes (4 bytes)
		/// </summary>
		public byte[] Mask
		{
			get {return this.mask;}
			set {
				
				if(value.Length!=4)
					throw new ArgumentException("The mask must be 4 bytes long","value");
				
				this.mask = value;
			}
		}
		
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
		
		public SocketAddress AsSocketAddress()
		{
			SocketAddress sa = new SocketAddress(AddressFamily.InterNetwork, this.Address.Length);
			for(int i=0;i<this.Address.Length;i++)
				sa[i]=this.Address[i];
			
			return sa;
		}
		
		/// <summary>
		/// Converts a string that represents 4 octects separated by dots to an
		/// array.
		/// </summary>
		public static byte[] ExtractOctects(string octectsAsString)
		{
			if(String.IsNullOrEmpty(octectsAsString))
				throw new ArgumentException("The argument can't be null or empty string","octectsAsString");
			else if(octectsAsString[0]=='.')
				throw new FormatException("The first octect is empty!. Empty octects not allowed");
			
			int pos = 0;
			int lastPos = 0;
			List<byte> result = new List<byte>();
			
			while(pos>=0){
				pos = octectsAsString.IndexOf('.',lastPos);
				if(pos==lastPos)
					throw new FormatException("Found empty octect!. Empty octects not allowed");
				if(pos>=0)
				{ //Get all bytes but the last one
					result.Add(Byte.Parse(octectsAsString.Substring(lastPos, pos - lastPos)));
					pos++;
					lastPos = pos;
				}
				else
				{ // Get the last byte
					result.Add(Byte.Parse(octectsAsString.Substring(lastPos)));
				}
			}
			return result.ToArray();					                                                
		}
		
		/// <summary>
		/// Parses an string as an IPv4 address. It allows mask specification.
		/// </summary>
		/// <remarks>
		/// The formats allowed are:
		/// - A.A.A.A/M.M.M.M
		/// - A.A.A.A/M
		/// - A.A.A.A
		/// Where every letter between dots are values from 0 to 255 (an octect)
		/// </remarks>
		public static IPv4RawAddress Parse(string addr)
		{
			if(String.IsNullOrEmpty(addr))
				throw new ArgumentException("Can't parse an empty or null string","addr");
			
			addr = addr.Trim();
		    int pos = addr.IndexOf('/');
		    
		    IPv4RawAddress result = new IPv4RawAddress();

	   	    if(pos>=0)
  	        {
				byte[] byteArr = ExtractOctects(addr.Substring(0,pos));
				
				if(byteArr.Length!=4)
					throw new FormatException("The IPv4 address must have 4 octects");
				
				result.address = byteArr;
				byteArr = ExtractOctects(addr.Substring(pos+1));
				
				//If we have only one byte is a short mask specification with the number
				//of consecutive bits in the mask. IE: 192.168.1.1/17 so the mask is
				//255.255.128.0
				if(byteArr.Length==1)
					result.SetMask(Convert.ToInt32(byteArr[0]));
				else if(byteArr.Length==4)
					result.mask = byteArr;
				else
					throw new FormatException("The mask has an invalid format. It can be an octect or 4 octects separated by dots.");
	        }
	        else
	        {
				byte[] byteArr = ExtractOctects(addr.Substring(0,pos));
				
				if(byteArr.Length!=4)
					throw new FormatException("The IPv4 address must have 4 octects");
				
				result.address = byteArr;
	        }
		     
		    return result;
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
        
		/// <summary>
		/// Initializes a new range with only one port
		/// </summary>
        public PortRange(short port)
        {
            this.startPort = port;
            this.endPort = port;
        }
        
		/// <summary>
		/// Initializes a new range with a start port and an end port
		/// </summary>
        public PortRange(short start, short end)
        {
            if(start<0)
                throw new ArgumentException("Bad port number: "+start,"start");
            else if(end<0)
                throw new ArgumentException("Bad port number: "+end,"end");
            else if(end < start)
				throw new ArgumentException("The start port must be lesser or equal than the end port","start, end");
			
			this.startPort = start;
			this.endPort = end;
        }
        
		/// <summary>
		/// Returns an string that represents the port range and with the format
		/// needed to be parsed by the <b>Parse</b> method.
		/// </summary>
        public override string ToString ()
        {
            return startPort+":"+endPort;
        }
		
		/// <summary>
		/// Compares two port ranges and returs true if they are the same range
		/// </summary>
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
		/// 5    => Port range from 5 to 5 (only one port) is the same as specifying 5:5
		/// 5:24 => Port range from 5 to 24
		/// :4   => Port range from 0 to 4. Is the same as specifying 0:4
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
				startp = endp;
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
