
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class DeleteRuleCommand: GenericCommand
	{
	    
	    private int ruleNum;
	    
	    public int RuleNum
	    {
	        get { return this.ruleNum;}
	        set
	        {
	            if(value<1)
	                throw new ArgumentException("The number must be greater or equal to 1!");
	            
	            this.ruleNum = value;
	        }
	    }
	    
	    public override string Value
	    {
	        get
	        {
	            if(this.ruleNum>=1)
	                return this.ChainName+" "+this.ruleNum;
	            else
	                return this.ChainName;
	        }
	    }
	    
	    public override bool MustSpecifyRule
	    {
	        get
	        {
	            if(this.ruleNum>=1)
	                return false;
	            else
	                return true;
	        }
	    }


		
		public DeleteRuleCommand()
		  :base(RuleCommands.DeleteRule)
		{
		    ruleNum = -1;
		}
	}
}
