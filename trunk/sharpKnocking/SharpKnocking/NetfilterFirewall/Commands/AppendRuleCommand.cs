
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class AppendRuleCommand: GenericCommand
	{
		
		public AppendRuleCommand()
		  :base(RuleCommands.AppendRule)
	    {
		      
	    }

	}
}
