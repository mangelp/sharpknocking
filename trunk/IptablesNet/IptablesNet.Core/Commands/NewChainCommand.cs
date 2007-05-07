
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the new chain command
	/// </summary>
	public class NewChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public NewChainCommand()
		  :base(RuleCommands.NewChain)
		{
		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
