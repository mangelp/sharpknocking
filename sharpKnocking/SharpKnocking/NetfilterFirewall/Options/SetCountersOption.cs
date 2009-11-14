
using System;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	
	public class SetCountersOption: GenericOption
	{
		
		public SetCountersOption()
		  :base(RuleOptions.SetCounters)
		{
		    throw new NotImplementedException("A lazy programmer didn't implemented "+
		                                      "this class properly.");
		}
		
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    throw new NotImplementedException("A lazy programmer didn't implemented "+
                                 "this class properly.");
		}



	}
}
