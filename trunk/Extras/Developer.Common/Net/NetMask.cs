// NetMask.cs
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

namespace Developer.Common.Net
{
	//TODO: This class must be revised along with SkIpAddress to work with
	//ipv6 addresses
	
	/// <summary>
	/// Network mask implementation for ip4 addresses
	/// </summary>
	public class NetMask
	{
	    private IPAddress longMask;
	    
	    /// <summary>
	    /// Gets/sets the mask as a ip address.
	    /// </summary>
	    /// <remarks>
	    /// The mask is a ip address used to define what bits are part of the
	    /// network address and what not. This is done setting these bits to
	    /// 1 and the rest to 0. A bit by bit logical and operation between
	    /// the mask and a address will show the network address.
	    /// </remarks>
	    public IPAddress LongMask
	    {
	        get { return this.longMask;}
	        set { this.longMask = value;}
	    }
	    
	    private int shortMask;
	    
	    /// <summary>
	    /// Gets/sets the mask as the number of bits (counting from the left)
	    /// that are used in the mask (set to 1)
	    /// </summary>
	    public int ShortMask
	    {
	        get {return this.shortMask;}
	        set
	        {
	            if(value<0 || value > 32)
	                throw new ArgumentException("Invalid value "+
	                        value+" for the mask. It must be between 0 and 32");
	            
	            this.shortMask = value;
	        }
	    }
	    
        /// <summary>
        /// Default constructor.
        /// </summary>
		public NetMask()
		{
		}
		
        /// <summary>
        /// Constructor. Inits the mask in short format.
        /// </summary>
        /// <remarks>
        /// The short format is the mask expressed as the number of consecutive
        /// bits from the left that are in the mask.
        /// </remarks>
		public NetMask(int shortMask)
		{
		    this.shortMask = shortMask;    
		}
		
        /// <summary>
        /// Constructor. Inits the mask in long format.
        /// </summary>
        /// <remarks>
        /// The long format is the mask expressed as a 4 bytes address where
        /// each bit if is set to 1 marks that bit as part of the mask.
        /// </remarks>
		public NetMask(IPAddress mask)
		{
		    this.longMask = mask;    
		}
		
        /// <summary>
        /// Creates a new netmask from a string.
        /// </summary>
		public static NetMask Parse(string mask)
		{
		    if(String.IsNullOrEmpty(mask))
		    {
		        throw new ArgumentException(
		                  "The value can't be null or String.Empty", "mask");
		    }
		    
		    NetMask result = new NetMask();
		    
		    int count = 0;
		    
		    for(int i=0;i<mask.Length;i++)
		    {
		        if(mask[i]=='.')
		        {
		            count++;    
		        }
		    }
		    
		    //If there is 4 periods we have a mask as a ip addres (in format)
		    //If there is 0 periods we should have at least 2 characters and
		    //then the number of bits in the mask.
		    //Any other scenario will throw a format exception.
		    if(count==4)
		    {
		        try
		        {
		            result.longMask = IPAddress.Parse(mask);
		        }
		        catch(FormatException fex)
		        {
		            throw new FormatException(
		                           "The mask is not a valid ip address", fex);
		        }
		    }
		    else if(count>0)
		    {
		        throw new FormatException("Invalid format for mask: "+mask);    
		    }
		    else if(count == 0)
		    {
		        if(mask.Length!=2)
		        {
		            throw new FormatException(
		                          "Invalid format for short mask: "+mask);
		        }
		        
	            try
	            {
	                 result.shortMask = Int32.Parse(mask);
	            }
	            catch(FormatException fex)
	            {
	                throw new FormatException(
	                               "The mask is not a valid number", fex);
	            }
		    }
		    
		    return result;
		}
	}
}
