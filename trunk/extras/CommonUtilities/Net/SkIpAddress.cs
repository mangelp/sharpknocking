// SkIpAddress.cs
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

namespace CommonUtilities.Net
{
    /// <summary>
    /// Implementation that allows to specify a ip-address and network mask
    /// </summary>
	public class SkIpAddress: IPAddress
	{
	    private NetMask mask;
	    
	    /// <summary>
	    /// Address netmask
	    /// </summary>
	    public NetMask Mask
	    {
	        get { return this.mask;}
	        set { this.mask = value;}
	    }
		
		/// <summary>
		/// Constructor. Inits the address.
		/// </summary>
		public SkIpAddress(byte[] addr)
		  :base(addr)
		{
		}
        
		/// <summary>
		/// Returns a new instance of a IpAddressRange object that represents
		/// the string.
		/// </summary>
		public new static SkIpAddress Parse(string inputStr)
		{
		    inputStr = inputStr.Trim();
		    int pos = inputStr.IndexOf('/');
		    
		    SkIpAddress result;
		    IPAddress addr;

	   	    if(pos>=0) {
				string ip = inputStr.Substring(0, pos);
				if(IPAddress.TryParse(ip,out addr)) {
					result = new SkIpAddress(addr.GetAddressBytes());
					string mask = inputStr.Substring(pos+1);
					result.Mask = NetMask.Parse(mask);
				} else
					throw new FormatException("The input string was not in a correct format");
			} else if (IPAddress.TryParse(inputStr, out addr)) {
				result = new SkIpAddress(addr.GetAddressBytes());
			} else 
				throw new FormatException("The input string was not in a correct format");
			
			return result;
		}
		
		/// <summary>
		/// Returns true if the string can be converted to an IpAddress range or
		/// false if not. If true sets the ip range in the out parameter.
		/// </summary>
		public static bool TryParse(string strRange, out SkIpAddress range)
		{
		    range = null;
		    
		    try
		    {
		        range = SkIpAddress.Parse(strRange);
		        return true;
		    }
		    catch(Exception)
		    {
		        return false;
		    }  
		}
	}
}
