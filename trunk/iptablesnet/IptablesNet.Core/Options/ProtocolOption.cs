
using System;

using IptablesNet.Net;
using IptablesNet.Core;

using SharpKnocking.Common;


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
		    
		    if(TypeUtil.IsAliasName(typeof(ProtocolType), strVal, out obj))
		    {
		        this.protocol = (ProtocolType)obj;
		        return true;
		    }
		    
		    errStr = "The value '"+strVal+"' can't be converted to any known protocol";
		    return false;
		}
		
		protected override string GetValuesAsString()
		{
			return TypeUtil.GetDefaultAlias (this.protocol);
		}
	}
}
