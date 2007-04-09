
using System;
using SharpKnocking.Common;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class TargetExtensionParameterEventArgs
	{
		
	    private TargetExtensionParameter item;
	    
	    public TargetExtensionParameter Item
	    {
	        get { return item;}
	        set { item = value;}
	    }
		
		public TargetExtensionParameterEventArgs()
		  :base()
		{
		}
		
		public TargetExtensionParameterEventArgs(TargetExtensionParameter item)
		  :base()
		{
		    this.item = item;
		}
	
	}
}
