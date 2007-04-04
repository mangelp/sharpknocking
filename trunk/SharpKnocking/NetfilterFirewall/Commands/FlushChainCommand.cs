
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class FlushChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
	    
	    public FlushChainCommand()
	      :base(RuleCommands.FlushChain)
		{
		}
	}
}
