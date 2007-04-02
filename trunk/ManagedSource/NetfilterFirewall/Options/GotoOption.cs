
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	/// <summary>
	/// Goto option
	/// </summary>
	public class GotoOption: GenericOption
	{
	    private string chainName;
	    
	    public string ChainName
	    {
	        get{ return this.chainName;}
	        set
	        {
	            if(Net20.StringIsNullOrEmpty(value))
	                throw new ArgumentException("The value can't be null or empty");
	            
	            this.chainName = value;
                this.ignoreNextValueUpdate = true;
                base.Value = value;
	        }
	    }
	    
		public GotoOption()
		  :base(RuleOptions.Goto)
		{
		}
		
		public override bool TryReadValues(string strVal, out string errStr)
		{
		    if(Net20.StringIsNullOrEmpty(strVal))
		    {
		        errStr = "The input string is null or empty";
		        return false;
		    }
		    
		    this.chainName = strVal;
		    errStr=String.Empty;
		    return true;
		}

	}
}
