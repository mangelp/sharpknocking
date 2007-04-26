
using System;

using IptablesNet.Core;

namespace IptablesNet.Core.Options
{
	
	
	public class FragmentOption: GenericOption
	{	    
		public FragmentOption()
		  :base(RuleOptions.Fragment)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    errStr="The fragment option doesn't allow any values";
		    return false;
		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
