
using System;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the rename chain command.
	/// </summary>
	/// <remarks>
	/// This command only works if the chain is a user-defined chain.
	/// </remarks>
	public class RenameChainCommand: GenericCommand
	{ 
	    private string newChain;
	    
		/// <summary>
		/// Gets sets the new name for the chain
		/// </summary>
	    public string NewChain
	    {
	        get { return this.newChain;}
	        set { this.newChain = value;}
	    }
	    
	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }

		
		public RenameChainCommand()
		  :base(RuleCommands.RenameChain)
		{
		}
		
		protected override string GetValuesAsString ()
		{
			 return this.newChain;
		}

	}
}
