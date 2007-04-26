using System;

namespace IptablesNet.Core.Commands
{
	
	/// <summary>
	/// Models the append rule command
	/// </summary>
	public class AppendRuleCommand: GenericCommand
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AppendRuleCommand()
		  :base(RuleCommands.AppendRule)
	    {
		      
	    }
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}

	}
}
