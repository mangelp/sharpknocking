
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class ZeroChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
		
		public ZeroChainCommand()
		  :base(RuleCommands.ZeroChain)
		{
		}
	}
}
