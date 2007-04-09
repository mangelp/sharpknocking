
using System;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class SetChainPolicyCommand: GenericCommand
	{
	    private string target;
	    
	    public string Target
	    {
	        get { return this.target;}
	        set { this.target = value;}
	    }
	    
	    public override string Value {
	    	get { return this.ChainName+" "+target; }
	    }
	    
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }

	    
	    public SetChainPolicyCommand()
	      :base(RuleCommands.SetChainPolicy)
		{
		}
	}
}
