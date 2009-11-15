// Structs.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 


using System;
using System.Net;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CommonUtilities.Net
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
		/// Start port of the range.
		/// </summary>
        public short StartPort
        {
            get { return this.startPort; }
        }
        
        private short endPort;
        
		/// <summary>
		/// End port of the range.
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
			//We return a string formatted for each case
			if(startPort == endPort)
				return startPort.ToString();
			else if(startPort == 0)
				return ":"+endPort;
			else
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
		
		/// <summary>
		/// Returns the hash code of the current port range
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
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
			//Console.WriteLine("Parsing range: "+range+" with char at pos: "+pos);
            
            if (pos < 0) {
				//Console.WriteLine("pos<0");
                //Only first port
                if (!Int16.TryParse(range, out startp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse to integer. Data: "+range);
                }
				//One port range
                endp = startp;
            } else if (pos == 0) {
				//Console.WriteLine("pos == 0");
                //Only last port
                if (!Int16.TryParse(range.Substring(1), out endp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse to integer. Data: "+range);
                }
				//The range starts in zero
				startp = 0;
			} else if (pos == (range.Length - 1)){
				throw new InvalidCastException("PortRange::Parse(string): Can't parse end port");
            } else {
				//Console.WriteLine("pos > 0");
				//Convert the first port
				if (!Int16.TryParse (range.Substring(0,pos),out startp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse start port to integer. Data: "+range);				
				}
				//Convert the last port
				if (!Int16.TryParse (range.Substring(pos+1),out endp)){
                    throw new InvalidCastException("PortRange::Parse(string): Can't "+
                                                   "parse end port to integer. Data: "+range);				
				}
			}

			//Swap port if are not in order
			if(startp>endp) {
				short temp = startp;
				startp = endp;
				endp = temp;
			}
			
            //Console.Out.WriteLine("Found ports ["+startp+", "+endp+"]");
            return new PortRange (startp, endp);
        }
    }
}
						