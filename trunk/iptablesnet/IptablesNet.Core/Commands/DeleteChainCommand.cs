
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the delete rule command
	/// </summary>
	public class DeleteChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
		
		/// <summary>
		/// Default constructor
		/// </summary>
	    public DeleteChainCommand()
	       :base(RuleCommands.DeleteChain)
		{

		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
