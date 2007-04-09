
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class RenameChainCommand: GenericCommand
	{
	    private string oldChain;
	    
	    public string OldChain
	    {
	        get { return this.oldChain;}
	        set { this.oldChain = value;}
	    }
	    
	    private string newChain;
	    
	    public string NewChain
	    {
	        get { return this.newChain;}
	        set { this.newChain = value;}
	    }
	    
	    public override string Value
	    {
	    	get { return this.oldChain+" "+this.newChain; }
	    }
	    
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }

		
		public RenameChainCommand()
		  :base(RuleCommands.RenameChain)
		{
		}
	}
}
