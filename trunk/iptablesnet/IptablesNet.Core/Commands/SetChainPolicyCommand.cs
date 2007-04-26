
using System;

using IptablesNet.Core;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the set chain policy command.
	/// </summary>
	public class SetChainPolicyCommand: GenericCommand
	{
	    private string target;
	    
	    public string Target
	    {
	        get { return this.target;}
	        set { this.target = value;}
	    }

	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }

	    public SetChainPolicyCommand()
	      :base(RuleCommands.SetChainPolicy)
		{
			throw new NotImplementedException ("This command is not implemented properly to be usable");
		}
		
		protected override string GetValuesAsString ()
		{
			return this.target;
		}

	}
}
