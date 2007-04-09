
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class EcnTargetExtension: TargetExtensionHandler
	{
		
		public EcnTargetExtension()
		  :base(typeof(EcnTargetOptions), "ecn")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new EcnParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    EcnParameter param = new EcnParameter(this);
		    
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(EcnParameter);
		}


		
		public class EcnParameter: TargetExtensionParameter
		{
		    public EcnParameter(EcnTargetExtension owner)
		      :base(owner)
		    {
		        
		    }
		    
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
