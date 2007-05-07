
using System;

using SharpKnocking.Common;
using IptablesNet.Core;

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// input interface option
	/// </summary>
	public class InInterfaceOption: GenericOption
	{
	    private string iface;
	    
	    public string Interface
	    {
	        get { return this.iface;}
	        set 
            { 
                this.iface = value;
            }
	    }

		public InInterfaceOption()
		  :base(RuleOptions.InInterface)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    if(!Net20.StringIsNullOrEmpty(strVal))
		    {
		        this.iface = strVal;
		        errStr=String.Empty;
		        return true;
		    }
		    
		    errStr = "The value can't be null or empty";
		    return false;
		}

		protected override string GetValuesAsString()
		{
			return this.iface;
		}

	}
}
