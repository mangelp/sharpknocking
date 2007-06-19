
using System;

using IptablesNet.Core;

using Developer.Common.Net;

namespace IptablesNet.Core.Options
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
		
		protected override string GetValuesAsString()
		{
			return this.address.ToString();
		}

	}
}
