
using System;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public class InsertRuleCommand: GenericCommand
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

		
		public InsertRuleCommand()
		  :base(RuleCommands.InsertRule)
		{
		    this.ruleNum = 1;
		}
	}
}
