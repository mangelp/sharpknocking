
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{	    
	/// <summary>
	/// Out interface option
	public class OutInterfaceOption: GenericOption
	{
	    
	    private string iface;
	    
	    public string Interface
	    {
	        get { return this.iface;}
	        set 
            { 
                this.iface = value;
                this.ignoreNextValueUpdate = true;
                base.Value = value;
            }
	    }
    
    
		public OutInterfaceOption()
		  :base(RuleOptions.OutInterface)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    if(!Net20.StringIsNullOrEmpty(strVal))
		    {
		        this.iface = strVal;
		        errStr = String.Empty;
		        return true;
		    }
		    
		    errStr = "The value can't be null or empty";
		    return false;
		}

	}
}
