
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the command to insert a new rule
	/// </summary>
	public class InsertRuleCommand: GenericCommand
	{
		//Default value. Insert rule at head of chain
	    private int ruleNum = 1;
	    
		/// <summary>
		/// Gets/Sets the position where to insert the rule
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
		
		public InsertRuleCommand()
		  :base(RuleCommands.InsertRule)
		{
		    this.ruleNum = 1;
		}
		
		protected override string GetValuesAsString ()
		{
			if(this.ruleNum == 1)
				return String.Empty;
			else
				return this.ruleNum.ToString();
		}

	}
}
