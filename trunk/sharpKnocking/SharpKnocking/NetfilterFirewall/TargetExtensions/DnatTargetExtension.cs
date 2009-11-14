
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class DnatTargetExtension: TargetExtensionHandler
	{
		
		public DnatTargetExtension()
		  :base(typeof(DnatTargetOptions), "dnat")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new DnatParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    TargetExtensionParameter param = new DnatParameter(this);
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(DnatParameter);
		}


		
		public class DnatParameter: TargetExtensionParameter
		{
		    public DnatParameter(DnatTargetExtension owner)
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
