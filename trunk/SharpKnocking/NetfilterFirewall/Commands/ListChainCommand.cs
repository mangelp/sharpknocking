
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class ListChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
	    
		public ListChainCommand()
		  :base(RuleCommands.ListChain)
		{
		}
	}
}
