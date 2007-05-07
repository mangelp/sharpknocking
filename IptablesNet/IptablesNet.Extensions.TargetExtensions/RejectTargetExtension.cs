
using System;

using SharpKnocking.Common;

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedTarget;

namespace IptablesNet.Extensions.ExtendedTarget
{
    /// <summary>
    /// Models the Reject target extension
    /// </summary>
	public class RejectTargetExtension: TargetExtensionHandler
	{
		
		public RejectTargetExtension()
		  :base(typeof(RejectTargetOptions), TargetExtensions.Reject)
		{
		}
        
        public override TargetExtensionParameter CreateParameter (string paramType)
        {
            object obj;
            
            if(!TypeUtil.IsAliasName (typeof (RejectTargetOptions), paramType, out obj))
                return null;
            
            RejectTargetOptions option = (RejectTargetOptions)obj;
            
            switch(option)
            {
                case RejectTargetOptions.RejectWith:
                    return new RejectParam(this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }
		
		public class RejectParam: TargetExtensionParameter
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
		    
		    public new RejectTargetOptions Option
		    {
		        get { return (RejectTargetOptions)base.Option;}
		    }
		    
		    public new RejectTargetExtension Owner
		    {
		        get { return (RejectTargetExtension)base.Owner;}
		    }
		    
		    public RejectParam(RejectTargetExtension owner)
		      :base(owner, RejectTargetOptions.RejectWith)
		    {}

            protected override string GetValuesAsString ()
            {
                return TypeUtil.GetDefaultAlias(this.rejectWith);
            }
            
            public override void SetValues (string value)
            {
                object enumValue;
                
                if(!TypeUtil.IsAliasName(typeof(RejectIcmpTypes), value, out enumValue))
                {
                    throw new FormatException("The value for --reject-with option is not"+
                            " a valid enumeration member of RejectIcmpTypes");
                }
                
                this.rejectWith = (RejectIcmpTypes)enumValue; 
            }


		    
		}
	}
}
