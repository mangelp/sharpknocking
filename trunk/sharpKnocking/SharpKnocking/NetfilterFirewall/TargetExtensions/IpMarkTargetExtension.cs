
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	public class IpMarkTargetExtension: TargetExtensionHandler
	{
		
		public IpMarkTargetExtension()
		  :base(typeof(IpMarkTargetOptions), "ipmark")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new IpMarkParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    IpMarkParameter param = new IpMarkParameter(this);
		    
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(IpMarkParameter);
		}


        public class IpMarkParameter: TargetExtensionParameter
        {
            public IpMarkParameter(IpMarkTargetExtension owner)
            :base(owner)
            {}
            
            protected override void ParseName (string name)
            {
            	throw new NotImplementedException();
            }
            
            protected override void ParseValue (string value)
            {
            	throw new NotImplementedException();
            }


        }
	}
}
