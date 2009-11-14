
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class NewChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
		
		public NewChainCommand()
		  :base(RuleCommands.NewChain)
		{
		}
	}
}
