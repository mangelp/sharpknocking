
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the flush chain command
	/// </summary>
	public class FlushChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
	    
		/// <summary>
		/// Default constructor
		/// </summary>
	    public FlushChainCommand()
	      :base(RuleCommands.FlushChain)
		{
		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
