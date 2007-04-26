
using System;

using SharpKnocking.Common;

using IptablesNet.Core.Extensions.ExtendedTarget;

namespace IptablesNet.Extensions.TargetExtensions
{
	public class RejectTargetExtension: TargetExtensionHandler
	{
		
		public RejectTargetExtension()
		  :base(typeof(RejectTargetOptions), "reject")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new RejectParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    RejectParameter par = new RejectParameter(this);
		    
		    par.Name = name;
		    par.Value = value;
		    
		    return par;
		}


		public override Type GetInternalParameterType ()
		{
			return typeof(RejectParameter);
		}

		
		public class RejectParameter: TargetExtensionParameter
		{
		    private RejectIcmpTypes rejectWith;
		    
		    /// <summary>
		    /// Icmp message type to return when a packet is rejected
		    /// </summary>
		    public RejectIcmpTypes RejectWith
		    {
		        get { return this.rejectWith;}
		        set { this.rejectWith = value;}
		    }
		    
		    private RejectTargetOptions option;
		    
		    public RejectTargetOptions Option
		    {
		        get { return this.option;}
		        set
		        {
		            this.option = value;
		            base.Name = TypeUtil.GetDefaultAlias(value);
		        }
		    }
		    
		    public new RejectTargetExtension Owner
		    {
		        get { return (RejectTargetExtension)base.Owner;}
		        set { base.Owner = (TargetExtensionHandler)value;}
		    }
		    
		    public RejectParameter(RejectTargetExtension owner)
		    :base(owner)
		    {}
		    
            protected override void ParseValue (string value)
            {
                object enumValue;
                
                if(!TypeUtil.IsAliasName(typeof(RejectIcmpTypes), value, out enumValue))
                {
                    throw new FormatException("The value for --reject-with option is not"+
                            " a valid enumeration member of RejectIcmpTypes");
                }
                
                this.rejectWith = (RejectIcmpTypes)enumValue;
            }
            
            protected override void ParseName (string name)
            {
                object objValue = this.Owner.ValidateAndGetParameter(name);
                this.option = (RejectTargetOptions)objValue;
            }

		    
		}
	}
}
