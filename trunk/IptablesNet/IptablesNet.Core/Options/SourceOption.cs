
using System;

using IptablesNet.Core;

using Developer.Common.Net;

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Destination address option
	/// </summary>
	public class SourceOption: GenericOption
	{
	    private IpAddressRange address;
	    
	    /// <summary>
	    /// Source address range to match
	    /// </summary>
	    public IpAddressRange Address
	    {
	        get { return this.address;}
	        set 
            { 
                this.address = value;
            }
	    }
		
		public SourceOption()
		  :base(RuleOptions.Source)
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
			return this.address.ToString ();
		}
	}
}
