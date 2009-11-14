
using System;

using SharpKnocking.Common.Net;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	/// <summary>
	/// Destination address option
	/// </summary>
	public class DestinationOption: GenericOption
	{
	    private IpAddressRange address;
	    
	    /// <summary>
	    /// Destination address range to match
	    /// </summary>
	    public IpAddressRange Address
	    {
	        get { return this.address;}
	        set 
            { 
                this.address = value;
                this.ignoreNextValueUpdate = true;
                base.Value = value.ToString();
            }
	    }
		
		public DestinationOption()
		  :base(RuleOptions.Destination)
		{
		      
		}
		
		public override bool TryReadValues (string strVal, out string errorStr)
		{
		    IpAddressRange range;
		    errorStr = String.Empty;
		    
		    if(IpAddressRange.TryParse(strVal, out range))
		    {
		        this.address = range;
		        return true;
		    }

	        errorStr = "Can't convert from string '"+strVal+"' to "+
	                   "object IpAddressRange";
	        return false;
		}

	}
}
