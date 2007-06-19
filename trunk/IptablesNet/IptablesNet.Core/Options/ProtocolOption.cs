
using System;

using IptablesNet.Core;

using Developer.Common.Net;
using Developer.Common.Types;


namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Models the protocol option
	/// </summary>
	public class ProtocolOption: GenericOption
	{   
	    private ProtocolType protocol;
	    
	    public ProtocolType Protocol
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
		    
		    if(AliasUtil.IsAliasName(typeof(ProtocolType), strVal, out obj))
		    {
		        this.protocol = (ProtocolType)obj;
		        return true;
		    }
		    
		    errStr = "The value '"+strVal+"' can't be converted to any known protocol";
		    return false;
		}
		
		protected override string GetValuesAsString()
		{
			return AliasUtil.GetDefaultAlias (this.protocol);
		}
	}
}
