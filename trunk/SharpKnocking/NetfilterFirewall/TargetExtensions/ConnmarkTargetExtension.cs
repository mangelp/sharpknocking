
using System;

using SharpKnocking.Common;

namespace SharpKnocking.IpTablesManager.RuleHandling.ExtendedTarget
{
	
	
	public class ConnmarkTargetExtension: TargetExtensionHandler
	{
		
		public ConnmarkTargetExtension()
          :base(typeof(ConnmarkTargetOptions), "connmark")
		{
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new ConnmarkParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    ConnmarkParameter param = new ConnmarkParameter(this);
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(ConnmarkParameter);
		}

		
		public class ConnmarkParameter: TargetExtensionParameter
		{
		    
		    
		    public ConnmarkParameter(ConnmarkTargetExtension owner)
		      :base(owner)
		    {
		        
		    }
		}


	}
}
