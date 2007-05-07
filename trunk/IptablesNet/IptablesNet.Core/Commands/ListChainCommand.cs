
using System;
using System.Text;

using SharpKnocking.Common;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the list chain rules command
	/// </summary>
	public class ListChainCommand: GenericCommand
	{
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }
	    
		public ListChainCommand()
		  :base(RuleCommands.ListChain)
		{
		}
		
		protected override string GetValuesAsString()
		{
			return String.Empty;
		}
	}
}
