
using System;

namespace IptablesNet.Net
{
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
