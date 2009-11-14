
using System;
using System.Net;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
    /// <summary>
    /// This  allows  you to DNAT connections in a round-robin way over a given
    /// range of destination addresses.
    /// </summary>
	public class BalanceTargetExtension: TargetExtensionHandler
	{
		public BalanceTargetExtension()
		  :base(typeof(BalanceTargetOptions), "balance")
		{
		      
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new BalanceParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    BalanceParameter result = new BalanceParameter(this);
		    result.Name = name;
		    result.Value = value;
		    
		    return result;
		}

		
		public override Type GetInternalParameterType ()
		{
			return typeof(BalanceParameter);
		}
		
		
		public class BalanceParameter: TargetExtensionParameter
		{
		    public new BalanceTargetExtension Owner
		    {
		        get { return (BalanceTargetExtension)base.Owner;}
		        set { base.Owner = (TargetExtensionHandler)value;}
		    }
		    
		    private BalanceTargetOptions option;
		    
		    public BalanceTargetOptions Option
		    {
		        get { return this.option;}
		        set
		        {
		            this.DisableInternalParsing = true;
		            this.option = value;
		            base.Name = TypeUtil.GetDefaultAlias(value);
		            this.DisableInternalParsing = false;
		        }
		    }
		    
		    private IPAddress start;
		    
		    /// <summary>
		    /// IP address start for the range
		    /// </summary>
		    public IPAddress Start
		    {
		        get { return this.start; }
		        set
		        {
		            this.UpdateValue();
		            this.start = value;
		        }
		    }
		    
		    private IPAddress end;
		    
		    /// <summary>
		    /// IP address end for the range
		    /// </summary>
		    public IPAddress End
		    {
		        get { return this.end;}
		        set
		        {
		            this.UpdateValue();
		            this.end = value;
		        }
		    }
		    
		    public BalanceParameter(BalanceTargetExtension owner)
		    :base(owner)
		    {
		        
		    }
		    
		    
		    private void UpdateValue()
		    {
		        //We must disable parsing to avoid infinite recursion
		        this.DisableInternalParsing = true;
		        base.Value = this.start+"-"+this.end;
		        this.DisableInternalParsing = false;
		    }
		    
		    protected override void ParseValue (string value)
		    {
		        int pos = this.Value.IndexOf('-');
		        
		        if(pos<0)
		            throw new FormatException(
		                 "The value has not the - for the ip range separator");
		        
		        this.start = IPAddress.Parse(this.Value.Substring(0, pos));
		        
		        this.end = IPAddress.Parse(this.Value.Substring(pos+1));
		    }
		    
		    protected override void ParseName (string name)
		    {
		        object objType = this.Owner.ValidateAndGetParameter(name);
		        
		        this.option = (BalanceTargetOptions)objType;
		    }
		    
		}
	}
}
