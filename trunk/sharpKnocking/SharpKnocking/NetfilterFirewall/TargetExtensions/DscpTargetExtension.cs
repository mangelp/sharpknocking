
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class DscpTargetExtension: TargetExtensionHandler
	{
		
		public DscpTargetExtension()
		  :base(typeof(DscpTargetOptions), "dscp")
		{
		}
		
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new DscpParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    DscpParameter param = new DscpParameter(this);
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}

        public override Type GetInternalParameterType ()
        {
        	return typeof(DscpParameter);
        }

		
		public class DscpParameter: TargetExtensionParameter
		{
		    public DscpParameter(DscpTargetExtension owner)
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
