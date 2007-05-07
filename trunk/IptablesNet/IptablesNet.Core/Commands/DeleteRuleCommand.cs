
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the delete rule command
	/// </summary>
	public class DeleteRuleCommand: GenericCommand
	{
		//Default value of -1
	    private int ruleNum = -1;
	    
		/// <summary>
		/// Rule number to delete
		/// </summary>
		/// <remarks>
		/// Set this property to a value less than 1 to disable it.
		/// </remarks>
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
	    
		/// <summary>
		/// Gets if the rule must be specified. It happens when the rule number is
		/// less than 1
		/// </summary>
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
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public DeleteRuleCommand()
		  :base(RuleCommands.DeleteRule)
		{
		    ruleNum = -1;
		}
		
		/// <summary>
		/// Returns the value of the command parameter as a string. 
		/// </summary>
		protected override string GetValuesAsString()
		{
			if(this.ruleNum>=1)
				return ruleNum.ToString();
			else
				return String.Empty;
		}
	}
}
