
using System;
using System.Net;

namespace Developer.Common.Net
{
    /// <summary>
    /// Implementation that allows to specify a ip-address and network mask
    /// </summary>
	[Obsolete("Will be removed in a future. Use an IpV4RawAddress or IpV6RawAddress structures instead")]
	public class IpAddressRange: IPAddress
	{
	    
	    private NetMask mask;
	    
	    /// <summary>
	    /// Address netmask
	    /// </summary>
	    public NetMask Mask
	    {
	        get { return this.mask;}
	        set {this.mask = value;}
	    }
		
		/// <summary>
		/// Default constructor. Inits the ip value to 0.0.0.0
		/// </summary>
		public IpAddressRange()
		  :base(Convert.ToInt64(0))
		{
		    
		}
		
		/// <summary>
		/// Constructor. Inits the addres.
		/// </summary>
		public IpAddressRange(byte[] addr)
		  :base(addr)
		{
		    
		}
		
		/// <summary>
		/// Constructor. Inits the address.
		/// </summary>
		public IpAddressRange(long addr)
		  :base(addr)
		{
		    
		}
        
		/// <summary>
		/// Returns a new instance of a IpAddressRange object that represents
		/// the string.
		/// </summary>
		public new static IpAddressRange Parse(string range)
		{
		    range = range.Trim();
		    int pos = range.IndexOf('/');
		    
		    IpAddressRange result;
		    IPAddress addr;
		    
		    try
		    {
        	    if(pos>=0)
       	        {
       	            string ip = range.Substring(0, pos);
       	            addr = IPAddress.Parse(ip);
       	            result = new IpAddressRange(addr.GetAddressBytes());
    	        
    	            string mask = range.Substring(pos+1);
    	            result.Mask = NetMask.Parse(mask);
    	        }
    	        else
    	        {
    	            addr = IPAddress.Parse(range);
    	            
    	            //This property is obsolete but if i use addr.GetAddressBytes this
    	            //method throws an exception. I don't know why but i'm now offline
    	            //and the bundled doc with mnodoc doesn't has entries for the key
    	            //methods of IpAddressRange.
    	            result = new IpAddressRange();
                    //TODO: This property is obsolete. Change this.
    	            result.Address = addr.Address;
    	        }
		     }
		     catch(Exception ex)
		     {
		        throw new FormatException("Invalid range format: "+addr, ex);
		     }
		     
		     return result;
		}
		
		/// <summary>
		/// Returns true if the string can be converted to an IpAddress range or
		/// false if not. If true sets the ip range in the out parameter.
		/// </summary>
		public static bool TryParse(string strRange, out IpAddressRange range)
		{
		    range = null;
		    
		    try
		    {
		        range = IpAddressRange.Parse(strRange);
		        return true;
		    }
		    catch(Exception)
		    {
		        return false;
		    }  
		}
	}
}
