
using System;

namespace IptablesNet.Core.Commands
{
	
	
	public class ZeroChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
		
		public ZeroChainCommand()
		  :base(RuleCommands.ZeroChain)
		{
			throw new NotImplementedException ("This command is not implemented properly to be usable");
		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
