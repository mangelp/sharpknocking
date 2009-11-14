
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	/// <summary>
	/// input interface option
	/// </summary>
	public class InInterfaceOption: GenericOption
	{
	    private string iface;
	    
	    public string Interface
	    {
	        get { return this.iface;}
	        set 
            { 
                this.iface = value;
                base.Value = value;
            }
	    }
        
        public override string Value {
        	get { return base.Value; }
        	set 
            { 
                this.iface = value;
                this.ignoreNextValueUpdate = true;
                base.Value = value;
            }
        }

	    
		public InInterfaceOption()
		  :base(RuleOptions.InInterface)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    if(!Net20.StringIsNullOrEmpty(strVal))
		    {
		        this.iface = strVal;
		        errStr=String.Empty;
		        return true;
		    }
		    
		    errStr = "The value can't be null or empty";
		    return false;
		}



	}
}
