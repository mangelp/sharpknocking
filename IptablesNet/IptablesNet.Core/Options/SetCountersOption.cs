
using System;

using IptablesNet.Core;

namespace IptablesNet.Core.Options
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

		protected override string GetValuesAsString()
		{
			throw new NotImplementedException("O_o");
		}

	}
}
