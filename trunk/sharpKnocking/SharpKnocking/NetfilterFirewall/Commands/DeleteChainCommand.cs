
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class DeleteChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
	    
	    public DeleteChainCommand()
	       :base(RuleCommands.DeleteChain)
		{

		}
		
		
	}
}
