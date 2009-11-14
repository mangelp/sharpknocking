
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class ReplaceRuleCommand: GenericCommand
	{
	    private int ruleNum;
	    
	    public int RuleNum
	    {
	        get
	        {
	            return this.ruleNum;
	        }
	        set
	        {
	            if(value<1)
	                throw new ArgumentException("The rules number must be greater"+
	                                                             " or equal than 1");
	            
	            this.ruleNum = value;
	        }
	    }
	    
	    public override string Value {
	        get
	        {
	           return this.ChainName+" "+this.ruleNum;    
	        }
	    }

	    
		public ReplaceRuleCommand()
		  :base(RuleCommands.ReplaceRule)
		{
		    this.ruleNum = 1;
		}
	}
}
