
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	/// <summary>
	/// This  target provides userspace logging of matching packets.  When this
    /// target is set for a rule, the Linux kernel will multicast  this  packet
    /// through a netlink socket. One or more userspace processes may then sub-
    /// scribe to various multicast groups and receive the packets.  Like  LOG,
    /// this  is  a  "non-terminating target", i.e. rule traversal continues at
    /// the next rule.
	/// </summary>
	public class UlogTargetExtension: TargetExtensionHandler
	{
		
		public UlogTargetExtension()
		  :base(typeof(UlogTargetOptions), "ulog")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new UlogParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    UlogParameter par = new UlogParameter(this);
		    
		    par.Name = name;
		    par.Value = value;
		    
		    return par;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(UlogParameter);
		}
		
		public class UlogParameter: TargetExtensionParameter
		{
		    public new UlogTargetExtension Owner
		    {
		        get { return (UlogTargetExtension)base.Owner; }
		        set { base.Owner = (TargetExtensionHandler)value;}
		    }
		    
		    private UlogTargetOptions option;
		    
		    public UlogTargetOptions Option
		    {
		        get { return this.option;}
		        set
		        {
		            this.option = value;
		            this.DisableInternalParsing = true;
		            this.Name = TypeUtil.GetDefaultAlias(value);
		            this.DisableInternalParsing = false;
		        }
		    }
		    
		    public UlogParameter(UlogTargetExtension owner)
		    :base(owner)
		    {}
		    
		    protected override void ParseValue (string value)
		    {
		        //We don't set nothing. Whe only check the format.
		        switch(this.option)
		        {
		            case UlogTargetOptions.UlogCprange:
		            case UlogTargetOptions.UlogQthreshold:
		                Int32.Parse(value);
		                break;
		            case UlogTargetOptions.UlogNlgroup:
		                int num = Int32.Parse(value);
		                if(num<1 || num> 32)
		                    throw new FormatException("The value can't be more"+
		                        " than 32 or less than 1(default).");
		                break;
		            case UlogTargetOptions.UlogPrefix:
		                if(value==null || value.Length>32)
		                    throw new FormatException("The value can't have more"+
		                        " than 32 characters.");
		                break;
		        }
		    }
		    
		    protected override void ParseName (string name)
		    {
		        object objType = this.Owner.ValidateAndGetParameter(name);
		        
		        this.option = (UlogTargetOptions)objType;
		    }
		    
		    /// <summary>
		    /// Returns the value of the parameter as an int
		    /// </summary>
		    /// <remarks>
		    /// This methods returns an int if the parameter value is of type
		    /// int. If the value can't be converted to an int or the type of
		    /// the parameter doesn't is of type int this will throw a
		    /// FormatException.
		    /// <remarks>
		    public int GetValueAsInt()
		    {
                if(option == UlogTargetOptions.UlogCprange || 
                   option == UlogTargetOptions.UlogQthreshold ||
                   option == UlogTargetOptions.UlogNlgroup)
                {
                    int num = Int32.Parse(this.Value);
                    return num;
                }
                else
                    throw new FormatException("This parameter doesn't have "+
                        "a value of type int.");
		    }
		    
		}
	}
}
