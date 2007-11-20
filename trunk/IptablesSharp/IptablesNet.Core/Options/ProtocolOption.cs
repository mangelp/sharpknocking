
using System;

using IptablesNet.Core;

using Developer.Common.Net;
using Developer.Common.Types;

using Developer.Common.Unix.Net;


namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Models the protocol option
	/// </summary>
	public class ProtocolOption: GenericOption
	{   
	    private Protocols protocol;
	    
	    public Protocols Protocol
	    {
	        get
	        {
	            return this.protocol;    
	        }
	        set
	        {
	            this.protocol = value;
	        }
	    }
	    
		public ProtocolOption()
		  :base(RuleOptions.Protocol)
		{
		}

		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    object obj;
		    errStr=String.Empty;
		    
		    if(AliasUtil.IsAliasName(typeof(Protocols), strVal, out obj))
		    {
		        this.protocol = (Protocols)obj;
		        return true;
		    }
		    
		    errStr = "The value '"+strVal+"' can't be converted to any known protocol";
		    return false;
		}
		
		protected override string GetValueAsString()
		{
			return AliasUtil.GetDefaultAlias (this.protocol);
		}
	}
}
