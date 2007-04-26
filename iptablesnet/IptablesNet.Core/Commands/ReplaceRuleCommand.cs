
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the replace rule command
	/// </summary>
	public class ReplaceRuleCommand: GenericCommand
	{
	    private int ruleNum;
	    
		/// <summary>
		/// Gets/Sets the rule number to replace
		/// </summary>
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
	    
		public ReplaceRuleCommand()
		  :base(RuleCommands.ReplaceRule)
		{
		    this.ruleNum = 1;
		}
		
		protected override string GetValuesAsString ()
		{
			return this.ruleNum.ToString();
		}

	}
}
