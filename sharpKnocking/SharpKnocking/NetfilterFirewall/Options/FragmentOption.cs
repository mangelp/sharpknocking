
using System;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	
	public class FragmentOption: GenericOption
	{
	    public override string Value
	    {
	        set
	        {
	           throw new InvalidOperationException("This option doesn't have a value");    
	        }
	    }
	    
		public FragmentOption()
		  :base(RuleOptions.Fragment)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    errStr="The fragment option doesn't allow any values";
		    return false;
		}
	}
}
