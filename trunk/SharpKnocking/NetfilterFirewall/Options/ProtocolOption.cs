
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	/// <summary>
	/// Models the protocol option
	/// </summary>
	public class ProtocolOption: GenericOption
	{
	    public override string Value
	    {
	    	get { return base.Value; }
	    	set
	    	{
	    	    base.Value = value;
	    	    this.OnValueSet();
	    	}
	    }

	    
	    private ProtocolType protocol;
	    
	    public ProtocolType Protocol
	    {
	        get
	        {
	            return this.protocol;    
	        }
	        set
	        {
	            this.protocol = value;
	        }
	    }
	    
		public ProtocolOption()
		  :base(RuleOptions.Protocol)
		{
		}

		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    object obj;
		    errStr=String.Empty;
		    
		    if(TypeUtil.IsAliasName(typeof(ProtocolType), strVal, out obj))
		    {
		        this.protocol = (ProtocolType)obj;
		        return true;
		    }
		    
		    errStr = "The value '"+strVal+"' can't be converted to any known protocol";
		    return false;
		}
		
		protected void OnValueSet()
		{
		    //Set here the needed information about the implicit extension
		    //loading.
		    switch(this.protocol)
		    {
		        case ProtocolType.Icmp:
		            this.SetImplicitExtension(MatchExtensions.Icmp);
		            break;
		        case ProtocolType.Tcp:
		            this.SetImplicitExtension(MatchExtensions.Tcp);
		            break;
		        case ProtocolType.Udp:
		            this.SetImplicitExtension(MatchExtensions.Udp);
		            break;
		    }
		}
        
	}
}
